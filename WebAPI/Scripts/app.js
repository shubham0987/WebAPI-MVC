var app = angular.module("myApp", []);

app.controller("myCtrl", function($scope, $http){
    
    $scope.selected = {};
    $scope.message = "";
    $scope.questions = "";

    $http.get("/api/questions").then(function (result) {
        $scope.questions = result.data;
    });
        $scope.getQuestions = function () {
            $http.get("/api/questions").then(function (result) {
                $scope.questions = result.data;
            });
        };

        $scope.delQues = function (id) {
            $http.delete("/api/questions/" + id).then(function (result) {
                $scope.message = "Question Deleted";
                $scope.getQuestions();
            });
        };

        $scope.editQues = function (question) {
            $scope.selected = angular.copy(question);
        };

        $scope.getTemplate = function (question) {
            if (question.quesID === $scope.selected.quesID)
                return 'Edit';
            else
                return 'Display';
        };

        $scope.reset = function () {
            $scope.selected = {};
        };

        $scope.updateQues = function () {
            var question = {
                quesID: $scope.selected.quesID,
                question1: $scope.selected.question1,
                postedOn: $scope.selected.postedOn
            };

            var request = $http({
                method: "put",
                url: "/api/questions/" + $scope.selected.quesID,
                data: question
            });

            request.then(function (data) {
                $scope.message = "Question Updated";
                $scope.reset();
                $scope.getQuestions();
            })
        };

        $scope.addQues = function () {
            var q = {
                question1: $scope.ques
            };
            var request = $http({
                method: "post",
                url: "/api/questions",
                data: q
            });
            request.then(function (result) {
                $scope.message = "Question Added";
                $scope.getQuestions();
            });
        };

});