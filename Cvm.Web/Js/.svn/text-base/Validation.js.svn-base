
/******************* 
EXTENDED VALIDATION: Overwrite asp.net validation methods
The following alters the way client side validation is shown on the client.
It highlights fields in which there is validation errors.

NB: This script must be included immediately after the standard asp.net validation script has been included, 
which could be in the beginning of the form element.
 **********************************/

function redefineValidator(validatorFunctionName,className) {
    var origValidatorFunction = window[validatorFunctionName];
    window[validatorFunctionName] = function (val) {
        var isOk = origValidatorFunction(val);
        var el = $('#' + val.controltovalidate);
        if (isOk) {
            el.removeClass(className)
        } else {
            el.addClass(className)
        }
        return isOk;
    }
}

redefineValidator("RequiredFieldValidatorEvaluateIsValid","valErrorReq");
redefineValidator("RegularExpressionValidatorEvaluateIsValid", "valErrorRegex");
redefineValidator("RangeValidatorEvaluateIsValid", "valErrorRange");

/******************* 
EXTENDED VALIDATION END
*/

