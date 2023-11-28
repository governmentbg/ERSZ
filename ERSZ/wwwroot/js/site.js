function JsonBGdate(value) {
    if (!value) {
        return '';
    }

    return moment(value).format("DD.MM.YYYY");
}

function JsonBGdatetime(value) {
    if (!value) {
        return '';
    }

    return moment(value).format("DD.MM.YYYY HH:mm:ss");
}

function refreshTable(dataTableID) {
    $(dataTableID).DataTable().ajax.reload(null, true);
    return true;
}


//Преобразува handlebars template, който е съдържание в контейнер с подадено име
function TemplateToHtml(countainer, data) {
    var source = $(countainer).html();

    return HandlebarsToHtml(source, data);
}

//Преобразува handlebars template, 
function HandlebarsToHtml(hbTemplate, data) {
    var template = Handlebars.compile(hbTemplate);

    return template(data);
}

Handlebars.registerHelper('eachData', function (context, options) {
    var fn = options.fn, inverse = options.inverse, ctx;
    var ret = "";

    if (context && context.length > 0) {
        for (var i = 0, j = context.length; i < j; i++) {
            ctx = Object.create(context[i]);
            ctx.index = i;
            ret = ret + fn(ctx);
        }
    } else {
        ret = inverse(this);
    }
    return ret;
});

Handlebars.registerHelper("math", function (lvalue, operator, rvalue, options) {
    lvalue = parseFloat(lvalue);
    rvalue = parseFloat(rvalue);

    return {
        "+": lvalue + rvalue
    }[operator];
});

Handlebars.registerHelper("date", function (date) {
    dateValue = date;

    if (!date || date == undefined) {
        return "/";
    }
    return moment(dateValue).format("DD.MM.YYYY");
})

Handlebars.registerHelper("dateTime", function (date) {
    dateValue = date;

    return moment(dateValue).format("DD.MM.YYYY HH:mm:ss");
})

//Зарежда съдържанието на резултата от PartialView в div-елемент
function requestContent(url, data, callback) {

    $.ajax({
        type: 'GET',
        //async: true,
        cache: false,
        url: url,
        data: data,
        success: function (data) {
            callback(data);
        }
    });
}


function fillCombo(items, combo, selected) {
    items = decodeItems(items);
    var tmlp = '{{#each this}}<option value="{{value}}" {{#if selected}}selected="selected"{{/if}}>{{text}}</option>{{/each}}';
    $(combo).html(HandlebarsToHtml(tmlp, setSetSelected(items, selected)));
}
function setSetSelected(items, selected) {
    if (items && (selected !== undefined) && (selected !== null)) {
        for (var i = 0; i < items.length; i++) {
            if (items[i].value === selected.toString()) {
                items[i].selected = true;
            }
        }
    }

    return items;
}


function requestCombo(url, data, combo, selected, callback) {
    requestGET_Json(url, data, function (items) {
        fillCombo(items, combo, selected);
        if (callback) {
            callback(combo);
        }
    });
}
function decodeText(text) {
    return $('<div/>').html(text).text();
}
function decodeItems(items) {
    if (items) {
        for (var i = 0; i < items.length; i++) {
            items[i].text = decodeText(items[i].text);
        }
    }
    return items;
}

function requestGET_Json(url, data, callback) {
    $.ajax({
        type: 'GET',
        async: true,
        cache: false,
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        url: url,
        data: data,
        success: function (data) {
            if (callback) {
                callback(data);
            }
        }
    });
}

function swalOk(text, callback) {
    Swal.fire({ title: 'Потвърди', text: text, type: 'warning' })
        .then(result => {
            if (result) {
                if (callback) {
                    callback();
                }
            } else {
                return false;
            }
        });
}
function swalConfirm(text, callback, cancelCallback, danger) {
    let icon = "warning";
    let dangerMode = false;
    let title = 'Потвърди';
    debugger;
    if (danger) {
        title = 'Внимание!';
        icon = "error";
        dangerMode = true;
    }

    Swal.fire({
        title: title,
        text: text,
        icon: icon,
        showConfirmButton: true,
        showCancelButton: true,
        confirmButtonText: 'Потвърди',
        cancelButtonText: "Отказ",
        dangerMode: dangerMode
    })
        .then((result) => {
            if (result.value) {
                callback();
            } else if (cancelCallback) {
                cancelCallback();
            } else {
                return false;
            }
        });
}

// Показва съобщения от JS
var messageHelper = (function () {
    function ShowErrorMessage(message) {
        toastr.error(message);
    }

    function ShowSuccessMessage(message) {
        toastr.success(message);
    }

    function ShowWarning(message) {
        toastr.warning(message);
    }

    return {
        ShowErrorMessage: ShowErrorMessage,
        ShowSuccessMessage: ShowSuccessMessage,
        ShowWarning: ShowWarning
    };
})();


function checkFilterFormHasData(filterContainer, minFilledCount) {
    if (!minFilledCount) {
        minFilledCount = 1;
    }
    let filledCount = 0;
    $(filterContainer).find('input[type="text"],input[type="number"]').each(function (i, e) {
        if ($(e).val() && $(e).val().length > 0 && $(e).val() != '0') {
            filledCount++;
        }
    });
    $(filterContainer).find('input.ui-autocomplete-input').parent().find('input[type="hidden"]').each(function (i, e) {
        if ($(e).val() && $(e).val().length > 0 && $(e).val() != '0') {
            filledCount++;
        }
    });
    $(filterContainer).find('select').each(function (i, e) {
        if ($(e).val() && $(e).val().length > 0 && $(e).val() > 0) {
            filledCount++;
        }
    });
    return filledCount >= minFilledCount;
}


var jsonPageSize = 10;
function MakeJsonPager(data, pager_container, page_no, callbackName, callbackNameArg) {
    var pageCount = Math.ceil(data.length / jsonPageSize);
    var pagerData = [];
    for (var i = 1; i <= pageCount; i++) {
        var _selected = false;
        if (i === page_no) {
            _selected = true;
        }
        var _new = { page: i, selected: _selected };
        pagerData.push(_new);
    }
    let carg = callbackNameArg || '';
    var pagerTemplate = '{{#each this}}<a href="#" class="btn {{#if selected}}light{{/if}}{{#unless selected}}dark{{/unless}}" onclick="' + callbackName + '({{page}}' + carg + ');return false;">{{page}}</a>{{/each}}';
    var pagerHtml = HandlebarsToHtml(pagerTemplate, pagerData);
    if (pageCount === 1)
        pagerHtml = '';
    $(pager_container).html(pagerHtml);

    var startRow = Math.max((page_no - 1) * jsonPageSize, 0);
    //if (page_no == 1)
    var endRow = startRow + jsonPageSize;

    return data.slice(startRow, endRow);
}

Handlebars.registerHelper("dateTimeMin", function (date) {
    let dateValue = date;
    return moment(dateValue).format("DD.MM.YYYY HH:mm");
});

Handlebars.registerHelper('numberFormat', function (value, options) {
    if (isNaN(value)) {
        return "";
    }
    // Helper parameters
    var dl = options.hash['decimalLength'] || 2;
    var ts = options.hash['thousandsSep'] || ',';
    var ds = options.hash['decimalSep'] || '.';

    // Parse to float
    var valueFloat = parseFloat(value);

    // The regex
    var re = '\\d(?=(\\d{3})+' + (dl > 0 ? '\\D' : '$') + ')';

    // Formats the number with the decimals
    var num = valueFloat.toFixed(Math.max(0, ~~dl));

    // Returns the formatted number
    return (ds ? num.replace('.', ds) : num).replace(new RegExp(re, 'g'), '$&' + ts);
});
