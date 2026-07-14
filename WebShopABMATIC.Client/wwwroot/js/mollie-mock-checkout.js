(function () {
  var idealBtn = document.getElementById("mollie-method-ideal");
  var cardBtn = document.getElementById("mollie-method-card");
  var idealPanel = document.getElementById("mollie-panel-ideal");
  var cardPanel = document.getElementById("mollie-panel-card");
  var payBtn = document.getElementById("mollie-pay-btn");
  if (!payBtn) {
    return;
  }

  function selectMethod(method) {
    var isCard = method === "card";
    if (idealBtn) idealBtn.classList.toggle("active", !isCard);
    if (cardBtn) cardBtn.classList.toggle("active", isCard);
    if (idealPanel) idealPanel.hidden = isCard;
    if (cardPanel) cardPanel.hidden = !isCard;
  }

  if (idealBtn) {
    idealBtn.addEventListener("click", function () {
      selectMethod("ideal");
    });
  }
  if (cardBtn) {
    cardBtn.addEventListener("click", function () {
      selectMethod("card");
    });
  }
  selectMethod("card");

  payBtn.addEventListener("click", function () {
    var returnUrl = payBtn.getAttribute("data-return-url");
    if (!returnUrl) {
      return;
    }
    payBtn.disabled = true;
    payBtn.textContent = "Processing…";
    setTimeout(function () {
      window.location.href = returnUrl;
    }, 900);
  });
})();