var KTest;
(function (KTest) {
    function areEqual(expected, actual, forIdentity) {
        if (forIdentity ? (actual !== expected) : (actual != expected)) {
            throw expected + " が期待されていましたが、実際の値は " + actual + " でした。";
        }
    }
    KTest.areEqual = areEqual;

    function writeMessage(name, message) {
        $("<div></div>")
            .append(createSpan(name + ": "))
            .append(createSpan(message == null ? "" : message))
            .appendTo(".page");
    }
    KTest.writeMessage = writeMessage;

    function writeValue(scriptText) {
        writeMessage(scriptText, eval(scriptText));
    }
    KTest.writeValue = writeValue;

    function runTest(testFuncName, argArray) {
        try {
            eval(testFuncName).apply(null, argArray);
            writeResult(testFuncName, true);
        } catch (e) {
            writeResult(testFuncName, false, e);
        }
    }
    KTest.runTest = runTest;

    function writeResult(name, isPassed, message) {
        $("<div></div>")
            .append(createSpan(name + ": "))
            .append(createSpan(isPassed ? "成功" : "失敗").addClass(isPassed ? "success" : "fail"))
            .append(createSpan(message == null ? "" : " --- " + message))
            .appendTo(".page");
    }

    function createSpan(text) {
        return $("<span></span>").text(text);
    }
})(KTest || (KTest = {}));
