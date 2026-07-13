window.storePasswordToggle = function (inputId, button) {
  var input = document.getElementById(inputId);
  if (!input || !button) return;
  var icon = button.querySelector("i");
  var show = input.type === "password";
  input.type = show ? "text" : "password";
  button.setAttribute("aria-label", show ? "Hide password" : "Show password");
  button.setAttribute("aria-pressed", show ? "true" : "false");
  if (icon) {
    icon.classList.toggle("bi-eye", !show);
    icon.classList.toggle("bi-eye-slash", show);
  }
};
