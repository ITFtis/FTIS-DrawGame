// 切換活動
var changeActivety = function () {
    $("#Activities").change(function () {
        submitBackgroundForm();
        $("#Prizes").empty();
        $.ajax({
            type: "POST",
            url: GetAllPrizesByActs,
            dataType: "json",
            data: { groupName: $("#Activities").val() },
            success: function (mems) {
                $.each(mems, function (i, member) {
                    $("#Prizes").append(
                        '<option value="' + member.Value + '">' + member.Text + "</option>"
                    );
                });
                getPrizeAndParticipantCount();
            },
            error: function (ex) {
                alert("Failed to retrieve states : " + ex);
            },
        }); // ajax end
        return false;
    }); // change end

}


//確認抽獎人數最小值
function checkmin() {
    var numDraws = $("#numDraws").val();
    if (numDraws <= 0)
        $("#numDraws").val(1);
}

//清除得獎表格
function clearWinnerTable() {
    const displayArea = document.querySelector('.display-content');
    displayArea.innerHTML = '';
    //input.value = ''
}

//清除得獎者
function clearWinners() {
    var numDraws = $("#numDraws").val();
    var activityId = $("#Activities").val();
    var prizeId = $("#Prizes").val();
    var fd = new FormData();
    fd.append('numDraws', numDraws);
    fd.append('ACTID', activityId);
    fd.append('PRIZE', prizeId);
    // 呼叫控制器的清除表格方法
    $.ajax({
        url: '@Url.Action("ClearWinners")',
        type: "POST",
        processData: false,
        contentType: false,
        data: fd,
        success: function (response) {
            // 清除成功後，清空表格並啟用開始抽獎按鈕
            $("#startButton").prop("disabled", false);
            clearWinnerTable();
            document.querySelector('input').value = ''; //清空input裡面的數值
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

//20231004, add by markhong 顯示/隱藏Menu
function hideBanner() {
    addEventListener('resize', () => {
        if (window.innerHeight == Math.round(screen.height / window.devicePixelRatio))
            $('.fixed-top').hide();
        else
            $('.fixed-top').show();
    })


};

// 超出範圍時自動刪除最前面出中獎名單
function autoDeleteFirstWinner() {
    var content = $(".display-content");
    console.log(content[0].scrollHeight + "\t" + content.innerHeight())
    if ((content[0].scrollHeight - 3) > content.innerHeight()) {
        var firstChild = $(".display-content").find('div:first');
        firstChild.hide({
            duration: 200, easing: 'linear', complete: function () {
                firstChild.remove();
            }
        })
    }
}


//切換背景圖案
function submitBackgroundForm() {
    var selectedValue = $("#Activities").val()
    var acttxt = $("#Activities :selected").text();

    $.post(UrlChangeBackground, { selectedValue: selectedValue }, function (data) {
        //console.log("data = " + data[0])
        //20231004, add by markhong 新增無活動背景圖
        if (data[0] == "") {
            $("body").css("background-image", "url('./images/NoAct.png')");
            $(".container").hide();
        }
        else {
            $("body").css("background-image", "url('" + data[0] + "')");
            $(".container").show();
        }

        //20231004, add by markhong 切換下拉示選單底色
        $(".dropdown option").css("background", data[1]);
        $(".display").css("color", data[2]);
    });
}

//移除標題圖檔
function RemoveTitlePng() {
    var ACTID = $("#Activities").find("option:selected").val() != "" ? " - " + $("#Prizes").find("option:selected").text() : "";
}