
var dropDownColorPickerHTML = '<select class="colorPicker">' +
    '<option value="#000000" style="background-color: Black;color: #FFFFFF;">Black</option>' +
    '<option value="#808080" style="background-color: Gray;">Gray</option>' +
    '<option value="#A9A9A9" style="background-color: DarkGray;">DarkGray</option>' +
    '<option value="#D3D3D3" style="background-color: LightGrey;">LightGray</option>' +
    '<option value="#FFFFFF" style="background-color: White;">White</option>' +
    '<option value="#7FFFD4" style="background-color: Aquamarine;">Aquamarine</option>' +
    '<option value="#0000FF" style="background-color: Blue;">Blue</option>' +
    '<option value="#000080" style="background-color: Navy;color: #FFFFFF;">Navy</option>' +
    '<option value="#800080" style="background-color: Purple;color: #FFFFFF;">Purple</option>' +
    '<option value="#FF1493" style="background-color: DeepPink;">DeepPink</option>' +
    '<option value="#EE82EE" style="background-color: Violet;">Violet</option>' +
    '<option value="#FFC0CB" style="background-color: Pink;">Pink</option>' +
    '<option value="#006400" style="background-color: DarkGreen;color: #FFFFFF;">DarkGreen</option>' +
    '<option value="#008000" style="background-color: Green;color: #FFFFFF;">Green</option>' +
    '<option value="#9ACD32" style="background-color: YellowGreen;">YellowGreen</option>' +
    '<option value="#FFFF00" style="background-color: Yellow;">Yellow</option>' +
    '<option value="#FFA500" style="background-color: Orange;">Orange</option>' +
    '<option value="#FF0000" style="background-color: Red;">Red</option>' +
    '<option value="#A52A2A" style="background-color: Brown;">Brown</option>' +
    '<option value="#DEB887" style="background-color: BurlyWood;">BurlyWood</option>' +
    '<option value="#F5F5DC" style="background-color: Beige;">Beige</option>' +
    '</select>';



var globalIndex = 1;
var elements = [];
var styles = [];
var colors = [];

function addRow() {
    var table = document.getElementById('tbody');
    var row = table.insertRow(-1);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    cell1.innerHTML = globalIndex;
    cell2.innerHTML = '<input type="button" id="btnDelete" value="X" onclick="deleteRow(this)">';
    cell3.innerHTML = "r" + globalIndex++;
    cell4.innerHTML = dropDownColorPickerHTML;
    remakeNodes();
    remakeLines();
}

function deleteRow(row) {
    var index = row.parentNode.parentNode.rowIndex;
    document.getElementById('tbody').deleteRow(index - 1);
    remakeNodes();
    remakeLines();
}

function remakeNodes() {
    var rows = getTableRows();
    elements = [];
    for (var i = 0; i < rows.length; i++) {
        var cells = rows[i].cells;
        var node = { data: { id: cells[2].innerText } };
        elements.push(node);
    }
}

function remakeLines() {
    var rows = getTableRows();
    for (var i = 0; i < rows.length; i++) {
        var src = rows[i].cells[2].innerText;
        for (var j = 0; j < rows.length; j++) {
            var tar = rows[j].cells[2].innerText;
            if (src === tar) continue
            var edge = { data: { id: src + tar, source: src, target: tar } };
            elements.push(edge);
        }
    }
}

function remakeStyles() {
    styles = [];
    var rows = getTableRows();
    styles.push({
        selector: 'node',
        style: {
            'background-color': '#666',
            'label': 'data(id)'
        }
    });
    styles.push({
        selector: 'edge',
        style: {
            'width': 3,
            'line-color': '#ccc'
        }
    });
    for (var i = 0; i < rows.length; i++) {
        var cells = rows[i].cells;
        var color = document.getElementsByClassName("colorPicker")[i].value;
        var s = {
            selector: 'node[id = "' + cells[2].innerText + '"]',
            style: {
                'background-color': color
            }
        }
        styles.push(s);
    }
}
function drawFromTable(elements) {
    remakeStyles();
    drawFromDb(elements);
}

function drawFromDb(elements) {
    var cy = cytoscape({
        container: document.getElementById('cy'),
        elements: elements,
        style: styles,
        layout: {
            name: 'grid',
            rows: 3
        }
    });
}

function prepareForDB() {
    colors = [];
    var rows = getTableRows();
    for (var i = 0; i < rows.length; i++) {
        var cells = rows[i].cells;
        var color = document.getElementsByClassName("colorPicker")[i].value;
        var elem = [cells[2].innerText, color];
        colors.push(elem);
    }
}

function getJSON() {
    var rows = getTableRows();
    var jsonString = "{";
    for (var i = 0; i < rows.length; i++) {
        var parentNode = rows[i].cells[2].innerText;
        jsonString += parentNode + ":{"
        for (var j = 0; j < rows.length - 1; j++) {
            var childNode = rows[j].cells[2].innerText;
            if (parentNode === childNode) continue;
            jsonString += childNode + ",";
        }
        var lastChildNode = rows[rows.length - 1].cells[2].innerText;
        if (parentNode != lastChildNode) {
            jsonString += lastChildNode;
        }
        jsonString += "}";
        if (i != rows.length - 1)
            jsonString += ", ";
    }
    jsonString += "}";
    var n = jsonString.length;
    jsonString = jsonString.slice(0, (n - 3)) + jsonString.slice((n - 2));
    console.log(jsonString);
}

function getTableRows() {
    var table = document.getElementById('tbody');
    var rows = table.rows;
    return rows;
}

function fillTheTable(cols) {
    document.getElementById("tbody").innerHTML = "";
    for (var i = 0; i < cols.length; i++) {
        var table = document.getElementById('tbody');
        var row = table.insertRow(-1);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);
        cell1.innerHTML = cols[i][0].replace('r', '');
        cell2.innerHTML = '<input type="button" id="btnDelete" value="X" onclick="deleteRow(this)">';
        cell3.innerHTML = cols[i][0];

        var temp = document.createElement('div');
        temp.innerHTML = dropDownColorPickerHTML;
        var colorSelecter = temp.firstChild;
        var ops = colorSelecter.options;
        for (var j = 0; j < ops.length; j++) {
            if (ops[j].value == cols[i][1]) {
                colorSelecter.selectedIndex = j;
                cell4.appendChild(colorSelecter);
                break;
            }
        }   
    }
    var rows = getTableRows();
    var newIndex = rows[rows.length - 1].cells[0].innerText;
    globalIndex = ++newIndex;
}