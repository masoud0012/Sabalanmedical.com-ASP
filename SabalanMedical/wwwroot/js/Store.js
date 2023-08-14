$(document).ready(function () {
    $("#selectType").on("change", LoadProductsByType);
    function LoadProductsByType(typeID) {
        var requestData = {
            typeId: this.value,
        }
        $.ajax({
            url: "/Products/GetFilteredProducts",
            method: "POST",
            data: requestData,
            success: async function (response) {
                $("#productBody").html(response)
            },
            error: function (xhr, status, error) {
                console.error("Error fetching data: " + error);
            }
        });
    }

    $("#searchEn").on("keyup", function () {

        var requestData = {
            searchKey: $("#searchEn").text(),
            typeId: $("#selectType").val(),
            searchBy:"ProductNameEn"
        }
        $.ajax({
            url: "/Products/GetFilteredProducts",
            method: "POST",
            data: requestData,
            success: async function (response) {
                $("#productBody").html(response)
                console.log(response);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching data: " + error);
            }
        });
    });

    $("#searchPr").on("keyup", function () {

        var requestData = {
            searchKey: $("#searchPr").text(),
            typeId: $("#selectType").val(),
            searchBy: "ProductNameFr"
        }
        $.ajax({
            url: "/Products/GetFilteredProducts",
            method: "POST",
            data: requestData,
            success: async function (response) {
                $("#productBody").html(response)
                console.log(response);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching data: " + error);
            }
        });
    });

})