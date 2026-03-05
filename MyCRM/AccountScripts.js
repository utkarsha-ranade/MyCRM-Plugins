// JavaScript source code
var Sdk = window.Sdk || {};
(
    function () {
        this.tickerOnChange = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var tickerSymbol = formContext.getAttribute("tickersymbol").getValue();

            if (tickerSymbol !== "TICSY") {
                formContext.getControl("tickersymbol").setNotification("Enter the Ticker Symbol as TICSY", "tickerInfoField");
            }
            else {
                formContext.getControl("tickersymbol").clearNotification("tickerInfoField");
            }

            if (tickerSymbol === "TICSY") {
                formContext.ui.setNotification("Ticker Symbol is set to USA", "INFO", "tickerInfoForm");
            }
            else {
                formContext.ui.clearNotification("tickerInfoForm");
            }
            //var expression = new RegExp("^\\+?[1-9]\\d{1,14}$");
            //if (!expression.test(mainPhone)) {
            //    alert("Please enter a valid phone number.");
            //}
        }
    }
).call(Sdk);