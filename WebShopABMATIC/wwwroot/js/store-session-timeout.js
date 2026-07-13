window.storeBrowserSession = (function () {
  var pingTimer = null;
  var started = false;
  var PING_MS = 25 * 1000;

  function post(url) {
    try {
      if (navigator.sendBeacon) {
        navigator.sendBeacon(url);
        return;
      }
    } catch (e) {
      // Fall through to fetch.
    }
    try {
      fetch(url, { method: "POST", credentials: "same-origin", keepalive: true });
    } catch (e2) {
      // Ignore.
    }
  }

  function suspend() {
    post("/account/store-session/suspend");
  }

  function resume() {
    try {
      fetch("/account/store-session/resume", {
        method: "POST",
        credentials: "same-origin",
        keepalive: true
      });
    } catch (e) {
      // Ignore.
    }
  }

  function ping() {
    try {
      fetch("/account/store-session/ping", {
        method: "POST",
        credentials: "same-origin",
        keepalive: true
      }).then(function (res) {
        if (res && res.status === 401) {
          window.location.href = "/account/logout?returnUrl=/";
        }
      });
    } catch (e) {
      // Ignore.
    }
  }

  function onPageHide() {
    suspend();
  }

  function onPageShow() {
    resume();
  }

  function start() {
    if (started) return;
    started = true;
    window.addEventListener("pagehide", onPageHide);
    window.addEventListener("pageshow", onPageShow);
    resume();
    ping();
    pingTimer = setInterval(ping, PING_MS);
  }

  function stop() {
    if (!started) return;
    started = false;
    window.removeEventListener("pagehide", onPageHide);
    window.removeEventListener("pageshow", onPageShow);
    if (pingTimer) {
      clearInterval(pingTimer);
      pingTimer = null;
    }
  }

  return { start: start, stop: stop, suspend: suspend, resume: resume };
})();

window.storeSessionTimeout = (function () {
  var IDLE_MS = 15 * 60 * 1000;
  var timer = null;
  var started = false;
  var events = ["mousemove", "mousedown", "keydown", "touchstart", "scroll", "click"];

  function logout() {
    stop();
    try {
      window.storeBrowserSession && window.storeBrowserSession.stop();
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
    try {
      window.storeBrowserSession && window.storeBrowserSession.start();
    } catch (e) {
      // Ignore.
    }
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

