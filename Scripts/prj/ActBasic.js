$(document).ready(function () {
    //douHelper.getField(douoptions.fields, 'Prizes').formatter =
    //    douHelper.getField(douoptions.fields, 'Participants').formatter = function (v) { return v ? JSON.stringify(v) : '-'; }

    var $_PrizeEditDataContainer = undefined;  //Prize編輯的容器
    var $_ParticipantEditDataContainer = undefined;  //Participant編輯的容器
    var Prizeoptions = undefined;  //Prize dou options
    var Participantoptions = undefined;  //Participant dou options
    var hasChangeDetails = false;//是否已變更Details資料

    //Master編輯容器加入Detail
    douoptions.afterCreateEditDataForm = function ($container, row) {
        var isAdd = JSON.stringify(row) == '{}';

        var $_oform = $("#_tabs");
        $_PrizeEditDataContainer = $('<table>').appendTo($_oform.parent());

        //var $_oform = $container.find(".data-edit-form-group");
        //$_PrizeEditDataContainer = $('<table>').appendTo($_oform.parent());
        //$_ParticipantEditDataContainer = $('<table>').appendTo($_oform.parent());

        //保留確定按鈕
        $container.find('.modal-footer button').hide();
        $container.find('.modal-footer').find('.btn-primary').show();

        //20230914, add by markhong 變更背景圖示大小
        $('div[data-field=BACKGROUND]').find('img').css('width', '320');
        //20230914, add by markhong 備註圖片格式
        var $bgtitle = $("label:contains('背景圖檔')")
        var $bg = $('div[data-field=BACKGROUND]').find($bgtitle);
        var remind = '<span class="text-danger fw-lighter"> (註：檔案大小<80K，解析度：1920*1080)</span>';
        $(remind).appendTo($bg);

        //20230914, add by markhong 下拉選單調色盤
        var ppp = $container.find('div[data-field=SELECTBG]');
        ppp.find('.field-content').removeClass('col-sm-12');
        ppp.find('.field-content').addClass('col-sm-10');
        ppp.find('.field-content').wrap('<div class="row"></div>');
        var btn = '<input class="field-content col-sm-2" type="color" id="colorPicker">';
        $(btn).appendTo($(ppp).find('.row'));
        const colorPicker = document.getElementById('colorPicker');
        colorPicker.value = row.SELECTBG;
        colorPicker.addEventListener('input', function () {
            const chosenColor = colorPicker.value;
            $('div[data-field=SELECTBG]').find("input:text").val(`${chosenColor}`);
        });

        //20231001, add by markhong 文字調色盤
        var ppp2 = $container.find('div[data-field=DISPLAYFONTCOLOR]');
        ppp2.find('.field-content').removeClass('col-sm-12');
        ppp2.find('.field-content').addClass('col-sm-10');
        ppp2.find('.field-content').wrap('<div class="row"></div>');
        var btn2 = '<input class="field-content col-sm-2" type="color" id="colorPicker2">';
        $(btn2).appendTo($(ppp2).find('.row'));
        const colorPicker2 = document.getElementById('colorPicker2');
        colorPicker2.value = row.DISPLAYFONTCOLOR;
        colorPicker2.addEventListener('input', function () {
            const chosenColor2 = colorPicker2.value;
            $('div[data-field=DISPLAYFONTCOLOR]').find("input:text").val(`${chosenColor2}`);
        });

        //20231001, add by markhong 按鈕調色盤
        var ppp3 = $container.find('div[data-field=BUTTONFONTCOLOR]');
        ppp3.find('.field-content').removeClass('col-sm-12');
        ppp3.find('.field-content').addClass('col-sm-10');
        ppp3.find('.field-content').wrap('<div class="row"></div>');
        var btn3 = '<input class="field-content col-sm-2" type="color" id="colorPicker3">';
        $(btn3).appendTo($(ppp3).find('.row'));
        const colorPicker3 = document.getElementById('colorPicker3');
        colorPicker3.value = row.BUTTONFONTCOLOR;
        colorPicker3.addEventListener('input', function () {
            const chosenColor3 = colorPicker3.value;
            $('div[data-field=BUTTONFONTCOLOR]').find("input:text").val(`${chosenColor3}`);
        });

        //取Prize dou option 並產編輯Dom
        $.getJSON($.AppConfigOptions.baseurl + 'Prize/GetDataManagerOptionsJson', function (_opt) { //取model option
            _opt.title = '獎項';
            //取消自動抓後端資料
            _opt.tableOptions.url = undefined;
            _opt.datas = row.Prizes;
          
            //初始options預設值
            douHelper.setFieldsDefaultAttribute(_opt.fields);//給預設屬性
            _opt.beforeCreateEditDataForm = function (drow, callback) {
                var isAdd = JSON.stringify(drow) == '{}';
                if (isAdd) {
                    drow.ACTID = row.ACTID;
                }
                callback();
            };
            //$_PrizeTable = $_PrizeEditDataContainer.douTable(_opt);
            //--------------------------------------TAB問題待解，先以獨立Table處理
            $_PrizeTable = $_PrizeEditDataContainer.douTable(_opt).on([$.dou.events.add, $.dou.events.update, $.dou.events.delete].join(' '), function (a, b, c) {
                //hasChangeDetails = true;
                //alert('ssss');
                //console.log(row.ACTID);
                var fd = new FormData();
                fd.append('ACTID', row.ACTID);
                //1.重新取餐與者清單
                $.ajax({
                    url: "../Activities/GetData2",
                    //data: sJson,
                    type: "POST",
                    async: false,
                    data: fd,
                    processData: false,
                    contentType: false,
                    success: function (result) {
                        //2.告知dou 更新清單資料 tableReload(datas)
                        //$("#_table").DouEditableTable('tableReload', result);
                        //$_ParticipantEditDataContainer.DouEditableTable('setOptions', datas);
                        $("#_table").DouEditableTable('tableReload', result);
                        //$("#_table").DouEditableTable('setOptions', result);
                    }
                });             
            });
            //--------------------------------------TAB問題待解，先以獨立Table處理
        });

        //--------------------------------------TAB問題待解，先以獨立Table處理
        //取Participant dou option 並產編輯Dom
        //$.getJSON($.AppConfigOptions.baseurl + 'Participant/GetDataManagerOptionsJson', function (_opt) { //取model option
        //    _opt.title = 'Participants';
        //    //取消自動抓後端資料
        //    _opt.tableOptions.url = undefined;
        //    _opt.datas = row.Participants;
        //    //初始options預設值
        //    douHelper.setFieldsDefaultAttribute(_opt.fields);//給預設屬性
        //    _opt.beforeCreateEditDataForm = function (drow, callback) {
        //        var isAdd = JSON.stringify(drow) == '{}';
        //        if (isAdd) {
        //            drow.ACTID = row.ACTID;
        //        }
        //        callback();
        //    };

        //    //實體Dou js
        //    $_ParticipantTable = $_ParticipantEditDataContainer.douTable(_opt)
        //});
        //產tab
        //helper.bootstrap.genBootstrapTabpanel($_PrizeEditDataContainer.parent(), undefined, undefined, ['活動資料', '獎項', '參與者清單'], [$_oform, $_PrizeEditDataContainer, $_ParticipantEditDataContainer]);
        //--------------------------------------TAB問題待解，先以獨立Table處理

        helper.bootstrap.genBootstrapTabpanel($_PrizeEditDataContainer.parent(), undefined, undefined, ['活動資料', '獎項'], [$_oform, $_PrizeEditDataContainer]);
        if (isAdd) {
            //tablist隱藏
            $('#_tabs').closest('div[class=tab-content]').siblings().hide();
        }
    }
    //20230915, add by markhong 把長出來的tablist刪除
    douoptions.afterUpdateServerData = function (row, callback) {
        $('[role=tablist]').empty();
        callback();
    }

    //還原已變更Details資料
    //douoptions.afterEditDataCancel = function (r) {
    //    if (hasChangeDetails)
    //        douoptions.updateServerData(r, function (result) {
    //            $_masterTable.DouEditableTable('updateDatas', result.data);//取消編輯，detail有可能已做一些改變，故重刷UI
    //        })
    //}

    $("#_table").DouEditableTable(douoptions); //初始dou table
});
