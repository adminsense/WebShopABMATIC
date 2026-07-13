window.storeAuthSession = (function () {
  var KEY = "ws.store.auth.tab";
  var FLAG = "_authSession";
  var CHANNEL = "ws.store.auth";

  function mark() {
    try {
      sessionStorage.setItem(KEY, "1");
    } catch (e) {
      // Private mode / blocked storage — ignore.
    }
  }

  function clear() {
    try {
      sessionStorage.removeItem(KEY);
    } catch (e) {
      // Ignore.
    }
    try {
      var bc = new BroadcastChannel(CHANNEL);
      bc.postMessage({ type: "logout" });
      bc.close();
    } catch (e) {
      // Ignore.
    }
  }

  function isMarked() {
    try {
      return sessionStorage.getItem(KEY) === "1";
    } catch (e) {
      return false;
    }
  }

  function consumeUrlFlag() {
    try {
      var url = new URL(window.location.href);
      if (url.searchParams.get(FLAG) === "1") {
        mark();
        url.searchParams.delete(FLAG);
        var next = url.pathname + (url.search ? url.search : "") + url.hash;
        window.history.replaceState(null, "", next || "/");
        return true;
      }
    } catch (e) {
      // Ignore.
    }
    return false;
  }

  function listenForLogout() {
    try {
      var bc = new BroadcastChannel(CHANNEL);
      bc.onmessage = function (e) {
        if (e && e.data && e.data.type === "logout") {
          try {
            sessionStorage.removeItem(KEY);
          } catch (err) {
            // Ignore.
          }
          if (window.location.pathname.indexOf("/sign-in") !== 0) {
            window.location.href = "/account/logout?returnUrl=/";
          }
        }
      };
    } catch (e) {
      // Ignore.
    }
  }

  listenForLogout();

  /**
   * Same-tab login sets the flag before redirect. Closing the browser clears
   * sessionStorage; a restored auth cookie without a live tab mark must sign out.
   * Other open tabs can vouch via BroadcastChannel so multi-tab stays signed in.
   */
  function ensureAlive() {
    if (consumeUrlFlag() || isMarked()) {
      mark();
      return Promise.resolve(true);
    }

    return new Promise(function (resolve) {
      var settled = false;
      var bc;
      try {
        bc = new BroadcastChannel(CHANNEL);
      } catch (e) {
        resolve(false);
        return;
      }

      function finish(ok) {
        if (settled) return;
        settled = true;
        try {
          bc.close();
        } catch (e) {
          // Ignore.
        }
        if (ok) mark();
        resolve(ok);
      }

      bc.onmessage = function (e) {
        if (e && e.data && e.data.type === "alive") {
          finish(true);
        }
      };
      bc.postMessage({ type: "ping" });
      setTimeout(function () {
        finish(false);
      }, 200);
    });
  }

  // Answer pings from other tabs while this tab holds a live session mark.
  try {
    var pingBc = new BroadcastChannel(CHANNEL);
    pingBc.onmessage = function (e) {
      if (e && e.data && e.data.type === "ping" && isMarked()) {
        pingBc.postMessage({ type: "alive" });
      }
    };
  } catch (e) {
    // Ignore.
  }

  return { mark: mark, clear: clear, ensureAlive: ensureAlive };
})();

window.storeSessionTimeout = (function () {
  var IDLE_MS = 15 * 60 * 1000;
  var timer = null;
  var started = false;
  var events = ["mousemove", "mousedown", "keydown", "touchstart", "scroll", "click"];

  function logout() {
    stop();
    try {
      window.storeAuthSession && window.storeAuthSession.clear();
    } catch (e) {
      // Ignore.
    }
    window.location.href =
      "/account/logout?returnUrl=" +
      encodeURIComponent(
        "/sign-in?error=" +
          encodeURIComponent("Your session expired after 15 minutes of inactivity. Please sign in again.")
      );
  }

  function reset() {
    if (!started) return;
    if (timer) clearTimeout(timer);
    timer = setTimeout(logout, IDLE_MS);
  }

  function onActivity() {
    reset();
  }

  function start(minutes) {
    stop();
    if (typeof minutes === "number" && minutes > 0) {
      IDLE_MS = minutes * 60 * 1000;
    }
    started = true;
    events.forEach(function (name) {
      document.addEventListener(name, onActivity, { passive: true, capture: true });
    });
    reset();
  }

  function stop() {
    started = false;
    if (timer) {
      clearTimeout(timer);
      timer = null;
    }
    events.forEach(function (name) {
      document.removeEventListener(name, onActivity, { capture: true });
    });
  }

  return { start: start, stop: stop, reset: reset };
})();
