
 function FormatStrBr(str)
 {
          var title = str;
          var indexOf =  title.lastIndexOf(",");  
          if(indexOf <= 0)
            return str;           
        
          return title.substring(0,indexOf) + ", <br/> " + title.substring(indexOf + 1) ;
                
    }

function Check_Click(objRef)
{
    //Get the Row based on checkbox
    var row = objRef.parentNode.parentNode;
//    if(objRef.checked)
//    {
//        //If checked change color to Aqua
//        row.style.backgroundColor = "aqua";
//    }
//    else
//    {    
//        //If not checked change back to original color
//        if(row.rowIndex % 2 == 0)
//        {
//           //Alternating Row Color
//           row.style.backgroundColor = "#C2D69B";
//        }
//        else
//        {
//           row.style.backgroundColor = "white";
//        }
//    }
    
    //Get the reference of GridView
    var GridView = row.parentNode;
    
    //Get all input elements in Gridview
    var inputList = GridView.getElementsByTagName("input");
    
    for (var i=0;i<inputList.length;i++)
    {
        //The First element is the Header Checkbox
        var headerCheckBox = inputList[0];
        
        //Based on all or none checkboxes
        //are checked check/uncheck Header Checkbox
        var checked = true;
        if(inputList[i].type == "checkbox" && inputList[i] != headerCheckBox && !inputList[i].disabled)
        {
            if(!inputList[i].checked)
            {
                checked = false;
                break;
            }
        }
    }
    headerCheckBox.checked = checked;
    
}
function CheckAll(objRef)
{
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i=0;i<inputList.length;i++)
    {
        //Get the Cell To find out ColumnIndex
        var row = inputList[i].parentNode.parentNode;
        if(inputList[i].type == "checkbox"  && objRef != inputList[i])
        {
            if (objRef.checked && !inputList[i].disabled)
            {
                //If the header checkbox is checked
                //check all checkboxes
                //and highlight all rows
                //row.style.backgroundColor = "aqua";
                inputList[i].checked=true;
            }
            else
            {
                inputList[i].checked=false;
            }
        }
    }
}
function isNumberKey(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
    return false;
    return true;
}
 function Comma(Num) { //function to add commas to textboxes
        Num += '';
        Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
        Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
        x = Num.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1))
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        return x1 + x2;
    }
    
    
function P5sCountGridViewMapRowsCheck(id,idTxt)
{
    //get reference of GridView control
   var grid = document.getElementById(id);
    //variable to contain the cell of the grid
   var cell;
   var count = 0;
    
    if (grid.rows.length > 0)
    {
        //loop starts from 1. rows[0] points to the header.
        for (i=1; i<grid.rows.length; i++)
        {
            //get the reference of first column
            cell = grid.rows[i].cells[0];
            
            //loop according to the number of childNodes in the cell
            for (j=0; j<cell.childNodes.length; j++)
            {           
                //if childNode type is CheckBox                 
                if (cell.childNodes[j].type =="checkbox")
                {
                //assign the status of the Select All checkbox to the cell checkbox within the grid
                    if(cell.childNodes[j].checked)
                        count++;
                }
            }
        }
    }
     
  
     
  
     
        if(count == 0)
        {
           alert("Không có khách hàng nào được chọn !.");
            return false;
        }
    
    
     var txtCommune = document.getElementById(idTxt).parentNode;
     var lines = txtCommune.innerText.split('\n');    
     
     if(txtCommune.innerText.length <= 0)
     {
        alert("Phường / Xã/ Thị Trấn Huyện bắt buộc chọn !.");
        return false;
     }
    
    
    var strAlert = count + " khách hàng sẽ được gán vào "+  lines[1] +". Vui lòng xác nhận bạn chắc chắn gán vào.";
        
    var confirmed = confirm(strAlert);
    if (confirmed == true)    
        return true;    
    else
        return false;  
    
}

   
function P5sCountGridViewUnMapRowsCheck(id, idTxt)
{
    //get reference of GridView control
   var grid = document.getElementById(id);
    //variable to contain the cell of the grid
   var cell;
   var count = 0;
    
    if (grid.rows.length > 0)
    {
        //loop starts from 1. rows[0] points to the header.
        for (i=1; i<grid.rows.length; i++)
        {
            //get the reference of first column
            cell = grid.rows[i].cells[0];
            
            //loop according to the number of childNodes in the cell
            for (j=0; j<cell.childNodes.length; j++)
            {           
                //if childNode type is CheckBox                 
                if (cell.childNodes[j].type =="checkbox")
                {
                //assign the status of the Select All checkbox to the cell checkbox within the grid
                    if(cell.childNodes[j].checked)
                        count++;
                }
            }
        }
    }
    if(count == 0)
    {
       alert("Không có khách hàng nào được chọn !.");
        return false;
    }
    
       
     var txtCommune = document.getElementById(idTxt).parentNode;
     var lines = txtCommune.innerText.split('\n');    
     
     if(txtCommune.innerText.length <= 0)
     {
        alert("Phường / Xã/ Thị Trấn Huyện bắt buộc chọn !.");
        return false;
     }
    
    
    var strAlert = count + " khách hàng sẽ được gỡ bỏ ra "+  lines[1] +". Vui lòng xác nhận bạn chắc chắn muốn gỡ ra.";
    
    
    var confirmed = confirm(strAlert);
    if (confirmed == true)    
        return true;    
    else
        return false;  
    
}