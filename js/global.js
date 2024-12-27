var newWindow = null;


var decimalpoint = '.';
var decimaldigit = 2;
var thousandsseparator = ',';


function openWindowchild(link) {
    if (newWindow && !newWindow.closed)
        newWindow.focus();
    else {
        var x = true;//window.confirm("Reload ulang Device?")
        if (x) {
            height = screen.height - 150;
            width = screen.width - 300;
            file = "";
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            //alert(height);
            //alert(width);

            file = link;

            if (!window.showModalDialog) { 
                newWindow = window.open(file, "addnote", "width=" + width + ",height=" + height + ",top=" + top + ", left=" + left + ",toolbar=no,scrollbars=yes,directories=no,status=no,menubar=no,resizable=no,location=no,modal=yes");
                newWindow.focus();
            }
            else
                window.showModalDialog(file, "addnote1", "dialogWidth:" + width + "px;dialogHeight:" + height +"px");

            return false;
        } else {
            return false;
        }

    }
}

function sysOpenWindow(pagename, titleReport) {
    //alert(pagename);
    var browser = navigator.appName;
    var val = Math.floor(Math.random() * 100000);
    var reportTitle = 'VR' + val.toString();
    if (browser == "Microsoft Internet Explorer") {
        window.opener = self;
    }

    height = screen.height - 150;
    width = screen.width - 150;

    window.open(pagename, reportTitle, 'width=' + width + ',height=' + height + ',toolbar=yes,location=no,resizable=yes,scrollbars=yes');
    window.moveTo(0, 0);
} 

function openWindowcashier(link) {
    if (newWindow && !newWindow.closed)
        newWindow.focus();
    else {
        var x = true;//window.confirm("Reload ulang Device?")
        if (x) {
            height = 500;//screen.height - 150;
            width = 850;//screen.width - 300;
            file = "";
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            //alert(height);
            //alert(width);

            file = link;
            newWindow = window.open(file, "addnote", "width=" + width + ",height=" + height + ",top=" + top + ", left=" + left + ",toolbar=no,scrollbars=yes,directories=no,status=no,menubar=no,resizable=no,location=no,modal=yes");
            newWindow.focus();
            return false;
        } else {
            return false;
        }

    }
}

function convertnum(n, ds, dd, ts, er, type) {
    if (!n) n = "0";
    ns = convertnumseparator(n, ds);
    if (!isNaN(ns)) {
        n = parseFloat(ns);
        m = n < 0 ? "-" : ""; 		//get minus
        n = n < 0 ? n * -1 : n; 		//set to positive(absolute)
        n = n.toFixed(dd); 	//set decimal digit
        ns = n.toString();
        n = ns.split('.')[0]; 	//get value
        dv = ns.split('.').length > 1 ? ns.split('.')[1] : ''; 	//get decimal value
        ds = dv != '' ? ds : ''
        if (ts != '')
            for (var i = 0; i < Math.floor((n.length - (1 + i)) / 3); i++) {
                n = n.substring(0, n.length - (4 * i + 3)) + ts + n.substring(n.length - (4 * i + 3));
            }
        if (type == "number")
            return m + n;
        return m + n + ds + dv;
    }
    retval = "invalid number";
    switch (er) {
        case 0:
            retval = 0.0;
            retval = retval.toFixed(dd);
            break;
    }
    return retval;
}

function convertnumseparator(n, ds) {
    switch (ds) {
        case ".":
            n = n.toString().replace(/\$|\,/g, '');
            break;
        case ",":
            n = n.toString().replace(/\$|\./g, '');
            n = n.toString().replace(/\$|\,/g, '.');
            break;
    }
    return n;
}

function openWindowchildnew(link) {
    var newWindow1 = null;
    
            height = screen.height - 200;
            width = screen.width - 350;
            file = "";
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (height / 2);
            //alert(height);
            //alert(width);
            file = link;
    newWindow1 = window.parent.open(file, "", "width=" + width + ",height=" + height + ",top=" + top + ", left=" + left + ",toolbar=no,scrollbars=yes,directories=no,status=no,menubar=no,resizable=no,location=no,modal=yes");
    //newWindow1.focus();
    return false;
    
}

function parent_disable() {
    if (newWindow && !newWindow.closed) {
        newWindow.focus();
    }
}

function formatNumber(n) {
    // format number 1000000 to 1,234,567
    if (n.indexOf("-") >= 0)
        return "-"+n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    else
        return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.
    // get input value
    //alert(formatNumber(input.value));
    //var input = document.getElementById("claimamount");	
    var input_val = input.value;
    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position 
    //var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        //input_val = "Rp" + left_side + "." + right_side;
        input_val = left_side + "." + right_side;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);
        //input_val = "Rp" + input_val;

        // final formatting
        if (blur === "blur") {
            input_val += ".00";
        }
    }

    // send updated string to input

    input.value = input_val;

}

function validationDelete() {
    var x = window.confirm("Are you sure want to delete ?")
    if (x) {
        return true;
    } else {
        return false;
    }
}  