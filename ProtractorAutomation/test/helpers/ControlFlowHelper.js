/**
 * Created by vyom.sharma on 14-02-2017.
 */

var ControlFlowHelper = function () {
    var JsonHelper = require('./JsonHelper.js');
    var jsonHelper = new JsonHelper();

    var InputHelper = require('./InputHelper.js');
    var inputHelper = new InputHelper();

    this.setControl = function (testData, testName, takeScreenShot, takeScreenShotBrowser, i, variableInLocatorIdentifierList) {
        browser.controlFlow().execute(function () {
            console.log("Inside pushInControlFlow testData= ");
            console.log(testData);
            console.log("variableInLocatorIdentifierList");
            console.log(variableInLocatorIdentifierList);
            for (var j = 0; j < variableInLocatorIdentifierList.length; j++) {
                var variableValue = jsonHelper.GetIndexedVariableValueFromVariableContainer(variableInLocatorIdentifierList[j].replace('{', '').replace('}', ''));
                console.log("variableValue= " + variableValue);
                testData.LocatorIdentifier = testData.LocatorIdentifier.replace(variableInLocatorIdentifierList[j], variableValue);
                console.log("new LocatorIdentifier = " + testData.LocatorIdentifier);
            }
            var key = inputHelper.setLocator(testData, testName, takeScreenShot, takeScreenShotBrowser, i);
        });
    };

};
module.exports = ControlFlowHelper;