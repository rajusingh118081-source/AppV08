validation.rules["phoneNumber"] = {
    message: "Phone number should like, +XX-XXXX-XXXX, + XX.XXXX.XXXX, + XX XXXX XXXX",
    method: function(el){
        //return el.value === "" || /^-?\d+$/.test(el.value);
        //var phoneno = /^\+?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$/;
        //if (el.value.match(phoneno))
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return true;
    }
}

validation.rules["addressMax"] = {
    message: "Address should be maximum 300 characters",
    method: function(el){
        var enteredText = el.value;
        numberOfLineBreaks = (enteredText.match(/\n/g) || []).length;
        characterCount = enteredText.length + numberOfLineBreaks;
        if (characterCount > 300) {            
            return false;
        }        
        return true;
    }
}
validation.rules["addressParagraph"] = {
    message: "Address should be maximum of 3 lines",
    method: function (el){
        var enteredText = el.value;
        numberOfLineBreaks = (enteredText.match(/\n/g) || []).length;
        if (numberOfLineBreaks > 2) {
            return false;
        }
        return true;
    }
}
validation.rules["addressParagraphMax"] = {
    message: "Address should be maximum of 100 characters in a line",
    method: function (el){
        var enteredText = el.value;
        var str = enteredText.replace(/\n/g, "$1|").split("|");
        if (str.length == 3) {
            for (var i = 0; i < str.length; i++) {
                if (str[i].length > 100) {
                    return false
                }
            }
        }        
        return true;
    }
}
validation.rules["specialInstructionMax"] = {
    message: "Special Instruction should be maximum of 256 characters",
    method: function (el){
        var enteredText = el.value;
        numberOfLineBreaks = (enteredText.match(/\n/g) || []).length;
        characterCount = enteredText.length + numberOfLineBreaks;
        if (characterCount > 256) {
            return false;
        }
        return true;
    }
}
validation.rules["numberAndDecimal"] = {
    message: "Enter a valid number",
    method: function (el){
        var enteredText = el.value;
        if (enteredText.length > 0) {
            var numberRegex = /^\d+(?:\.\d{1,2})?$/;
            if (enteredText.match(numberRegex)) {
                var x = parseInt(enteredText);
                var y = parseInt(2147483647);
                if (x < y) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }                
        return true;
    }
}
validation.rules["maxNumber"] = {
    message: "Enter a valid number",
    method: function (el){
        var enteredText = el.value;
        if (enteredText.length > 0) {
            var x = parseInt(enteredText);
            var y = parseInt(2147483647);
            if (x < y) {
                return true;
            }
            else {
                return false;
            }
        }
        return true;
    }
}
validation.rules["maxDecimal"] = {
    message: "Enter a valid number",
    method: function (el){
        var enteredText = el.value;
        if (enteredText.length > 0) {
            var x = parseFloat(enteredText);
            var y = parseFloat(1.7976931348623157E+308);
            if (x < y) {
                return true;
            }
            else {
                return false;
            }
        }
        return true;
    }
}

validation.rules["emailValidate"] = {
    message: "Enter a valid email",
    method: function (el) {
        var enteredText = el.value;
        if (enteredText.length > 0) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (regex.test(enteredText)) {
                return true;
            }
            else {
                return false;
            }
        }
        return true;
    }
}
//validation.rules["quantityValidate"] = {
//    message: "Allow only numbers",
//    method: function (el) {
//        var enteredText = el.value;
//        if (enteredText.length > 0) {
//            var regex = /^[0-9]\d{0,9}(\.\d{1,4})?%?$/;
//            if (regex.test(enteredText)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//validation.rules["discountValidate"] = {
//    message: "Allow only numbers",
//    method: function (el) {
//        var enteredText = el.value;
//        if (enteredText.length > 0) {
//            var regex = /^-?[0-9]\d{0,9}(\.\d{1,4})?%?$/;
//            if (regex.test(enteredText)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//validation.rules["unitpriceValidate"] = {
//    message: "Allow only numbers",
//    method: function (el) {
//        var enteredText = el.value;
//        if (enteredText.length > 0) {
//            var regex = /^-?[0-9]\d{0,9}(\.\d{1,4})?%?$/;
//            if (regex.test(enteredText)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }
//        return true;
//    }
//}
//validation.rules["totalweightValidate"] = {
//    message: "Allow only numbers",
//    method: function (el) {
//        var enteredText = el.value;
//        if (enteredText.length > 0) {
//            var regex = /^[0-9]\d{0,9}(\.\d{1,4})?%?$/;
//            if (regex.test(enteredText)) {
//                return true;
//            }
//            else {
//                return false;
//            }
//        }
//        return true;
//    }
//}