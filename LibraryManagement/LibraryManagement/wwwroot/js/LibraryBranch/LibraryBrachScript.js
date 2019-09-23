$("#branchHourSubmitbtn").on('click', function () {
    ShowDataForQuestion();
    
});


function ShowDataForQuestion() {
    
    var selectedItem = getQuesSelectedItem();
    var index = $("#BranchHourCreateBody").children("tr").length;
    var sl = index;
    var IndexId = "<td style='display:none'><input type='hidden' id='Index" + index + "'name=BranchHourList.Index' Value='" + index + "' /> </td>";
    var SlId = "<td id='Sl" + index + "'>" + (++sl) + "</td>";
    var Dayofweek = "<td> <input type='hidden' id='DayOfWeek" + index + "'name='BranchHourList[" + index + "].DayOfWeek' value='" + selectedItem.DayOfWeek + "'/>" + selectedItem.DayOfWeek + "</td>";
    var Opentime = "<td> <input type='hidden' id='OpenTime" + index + "'name='BranchHourList[" + index + "].OpenTime' value='" + selectedItem.OpenTime + "'/>" + selectedItem.OpenTime + "</td>";
    var Closetime = "<td> <input type='hidden' id='CloseTime" + index + "'name='BranchHourList[" + index + "].CloseTime' value='" + selectedItem.CloseTime + "'/>" + selectedItem.CloseTime + "</td>";
    var newRow = "<tr>" + SlId + IndexId + Dayofweek + Opentime + Closetime + "</tr>";
    $("#BranchHourCreateBody").append(newRow);
    //$("#DayOfWeek").val("");
    //$("#OpenTime").val("");
    //$("#CloseTime").val("");

}

function getQuesSelectedItem() {
    var dayofweekNew = $("#DayOfWeek").val();
    var dayofweek = parseInt(dayofweekNew);
    var opentimeeNew = $("#OpenTime").val();
    var opentime = parseInt(opentimeeNew);
    var closetimeNew = $("#CloseTime").val();
    var closetime = parseInt(closetimeNew);
   
   
    var item = {
        "DayOfWeek": dayofweek,
        "OpenTime": opentime,
        "CloseTime": closetime
    }
    return item;
}