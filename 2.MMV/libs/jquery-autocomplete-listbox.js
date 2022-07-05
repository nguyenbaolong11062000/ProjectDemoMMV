
function eventListBoxChange(parentId,childId)
{


    var parent = document.getElementById(parentId);
    var parentSelected = new Array();
    var lengthParent= parent.options.length;
    
    for (var i = 0; i < parent.options.length; i++) 
    {
         if(parent.options[i].selected == true )
         {
             parentSelected.push(parent.options[i].value);
          }
     }   
     
     
    for (var i = lengthParent - 1; i>=0; i--)
    {
        if(parent.options[i].getAttribute("selected") == "selected")   
             parent.options[i].removeAttribute("selected"); 
    }  
    
    
    if(childId == "")
       return;    
        
    var child = document.getElementById(childId);
    var lengthChild = child.options.length;
    
    
    
      if(document.getElementById(childId + "_hdf") == null )
        return;
        
        
    var value = document.getElementById(childId + "_hdf").value;
    if(value == null || value == "")
        return;


    var childSelected = new Array();
    for (var i = lengthChild - 1; i >=0 ; i--)
    { 
         if(child.options[i].getAttribute("selected") == "selected")   //lưu thông tin các item dc set default value
         {
             childSelected.push(child.options[i].value);
         }
    }    
    
        
    for (var i = lengthChild - 1; i>=0; i--)
    { 
        child.remove(i);    
    }    
    
    var result =  $.parseJSON(value);  
                   
    for(var j = 0; j < result.length; j++)
    {  
       for(var i = 0; i < parentSelected.length; i++)
       {
            if(result[j].PARENT == parentSelected[i])
             {
                    var option = document.createElement("option");
                    option.text = result[j].NAME;
                    option.value = result[j].CD;
                    option.setAttribute("parentid",result[j].PARENT);
                    //set selected 
                    for(var k = 0; k < childSelected.length; k++)
                    {
                         if(childSelected[k] == result[j].CD)
                         {
                       
                             option.setAttribute("selected","selected");  
                             break;  
                         }
                    }                                   
                    child.add(option);                
                    
                    break;
             }
        }
     }  
    
    if ("createEvent" in document) 
    {
        var evt = document.createEvent("HTMLEvents");
        evt.initEvent("change", false, true);
        child.dispatchEvent(evt);
    }
    else
        child.fireEvent("onchange");
        
 
    // $("#"+childId).change(); //change child control 
}
  

function eventListBoxLoadFirstTime(parentId,childId)
{       
        
    var parent = document.getElementById(parentId)
    //check call back javascript

   
    var parentSelected = new Array();
    var lengthParent= parent.options.length;
    
    for (var i = 0; i < parent.options.length; i++) 
    {
         if(parent.options[i].selected == true )
         {
             parentSelected.push(parent.options[i].value);
          }
     }   
     
     
    for (var i = lengthParent - 1; i>=0; i--)
    {
        if(parent.options[i].getAttribute("selected") == "selected")   
             parent.options[i].removeAttribute("selected"); 
    }  
    
    
    if(childId == "")
       return;    
        
        
    var child = document.getElementById(childId);
    
    //hàm check để ngăn chặn eventListBoxLoadFirstTime load nhiều lần
    if( (parent.getAttribute("firstLoad") != null && parent.getAttribute("firstLoad") == "true" ) ||  
        ( child.getAttribute("firstLoad") != null && child.getAttribute("firstLoad") == "true")  )
    { 
        return;
    }  
    
    child.setAttribute("firstLoad","true"); 
    
    
    var lengthChild = child.options.length;
    
    if(document.getElementById(childId + "_hdf") == null )
        return;
        
        
    var value = document.getElementById(childId + "_hdf").value;
    if(value == null || value == "")
        return;


    var childSelected = new Array();
    for (var i = lengthChild - 1; i >=0 ; i--)
    { 
         if(child.options[i].getAttribute("selected") == "selected")   //lưu thông tin các item dc set default value
         {
             childSelected.push(child.options[i].value);
         }
    }    
    
    
    
    for (var i = lengthChild - 1; i>=0; i--)
    { 
        child.remove(i);    
    }    
    
    var result =  $.parseJSON(value);  
                   
    for(var j = 0; j < result.length; j++)
    {  
       for(var i = 0; i < parentSelected.length; i++)
       {
            if(result[j].PARENT == parentSelected[i])
             {
                    var option = document.createElement("option");
                    option.text = result[j].NAME;
                    option.value = result[j].CD;
                    option.setAttribute("parentid",result[j].PARENT);
                    //set selected 
                    for(var k = 0; k < childSelected.length; k++)
                    {
                         if(childSelected[k] == result[j].CD)
                         {
                             option.setAttribute("selected","selected");  
                             break;  
                         }
                    }                                   
                    child.add(option);                
                    
                    break;
             }
        }
     }  
 
    
    if ("createEvent" in document) 
    {
        var evt = document.createEvent("HTMLEvents");
        evt.initEvent("change", false, true);
        child.dispatchEvent(evt);
    }
    else
        child.fireEvent("onchange");
    
    
  //  child.fireEvent("onchange");
    // $("#"+childId).change(); //change child control
     
}


    
    
   

 //code run only Desktop, mobile error
//function eventListBoxLoadFirstTime(parentId,childId)
//{
// 
//    if(childId == "")
//        return;
//        
//    var parent = document.getElementById(parentId);
//    var parentSelected = new Array();
//   
//    for (var i = 0; i < parent.options.length; i++) 
//    {
//         if(parent.options[i].selected ==true)
//         {
//             parentSelected.push(parent.options[i].value);
//          }
//     }
//   
//   
//        
//    var child = document.getElementById(childId);
//    var lengthChild = child.options.length;

//    for (var i = lengthChild - 1; i>=0; i--)
//    {
//      child.options[i].setAttribute("style", "display:none"); 
//    }  
//    
//         
//    for(var i = 0; i < parentSelected.length; i++)
//    {         
//         for (var j = lengthChild - 1; j >=0; j--)
//         {
//             if(child.options[j].getAttribute("ParentId") == parentSelected[i])             
//                child.options[j].setAttribute("style", "display:block"); 
//         }  
//    }    
//   
//    $("#"+childId).change(); //change child control
//    
//}


    
