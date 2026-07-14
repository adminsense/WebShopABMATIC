window.storeSessionTimeout = (function () {
  var IDLE_MS = 15 * 60 * 1000;
  var timer = null;
  var started = false;
  var events = ["mousemove", "mousedown", "keydown", "touchstart", "scroll", "click"];

  function logout() {
    stop();
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
