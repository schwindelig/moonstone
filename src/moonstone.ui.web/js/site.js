var ms_profile_info = null;

$(document).ready(function () {
    initSemanticUi();
    initFlatpickr();
    initToastr();
});

function initSemanticUi() {
    // api
    $.fn.api.settings.api = {
        'api-login': '/user/login',
        'api-create-bankaccount': '/bankaccount/create',
        'api-create-category': '/category/create',
        'api-create-group': '/group/create',
        'api-create-transaction': '/transaction/create',
        'api-get-profile-info': '/User/GetProfileInformation',
        'api-get-categories-for-current-group': '/category/GetAllCategoriesForCurrentGroup',
        'api-get-bankaccounts-for-current-group': '/bankaccount/GetAllBankAccountsForCurrentGroup',
        'api-get-currencies': '/currency/GetAllCurrencies'
    };
    $.fn.api.settings.successTest = function (response) {
        if (response && response.success) {
            return response.success;
        }
        return false;
    };
    $.fn.api.settings.onSuccess = handleApiSuccess;
    $.fn.api.settings.onFailure = handleApiFailure;

    // get profile information
    $('body').api({
        action: 'api-get-profile-info', on: 'now',
        onSuccess: function (response) {
            console.log('success');
        },
        onFailure: function (response) {
            // user probably not signed in
        }
    });

    // nav dropdowns
    $('.ms-nav-profile-dropdown').dropdown();
    $('.ms-nav-group-dropdown').dropdown();

    // checkboxes
    $('ui.checkbox').checkbox();

    // icon toolbar tooltips
    $(".ms-icon-bar .item").popup();
    console.log('semantic initialized');
}

function initFlatpickr() {
    flatpickr('.ms-date-selector');
    console.log('flatpickr initialized');
}

function initToastr() {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function bindFormSubmit(formSelector, apiAction, callbackSuccess, callbackFailure) {
    var apiSettings = {
        action: apiAction,
        method: 'POST',
        serializeForm: true,
        //stateContext: formSelector
    };

    if (callbackSuccess !== null) {
        apiSettings.onSuccess = callbackSuccess;
    }

    if (callbackFailure !== null) {
        apiSettings.onFailure = callbackFailure;
    }

    $(formSelector + ' .submit.button').api(apiSettings);

    // jquery validate
    //$('input[data-val=true]').on('blur', function () {
    //    $(this).valid();
    //});
    $('.submit.button').on('click', function () {
        $(formSelector).valid();
    });
}

function handleApiFailure(response) {
    displayMessage('error', 'Error', response.message);
}

function handleApiSuccess(response) {
    //displayMessage('success', 'Success', response.message);

    if (response.returnUrl !== null) {
        window.location = response.returnUrl;
    }
}

function displayMessage(type, title, message) {
    switch (type) {
        case 'error':
            //toastr.error(message, title);
            toastr.error(message);
            break;
        case 'warning':
            //toastr.warning(message, title);
            toastr.warning(message);
            break;
        case 'success':
            toastr.success(message);
            break;
        case 'info':
        default:
            //toastr.info(message, title);
            toastr.info(message);
    }
}

function initModal(modalSelector, triggerSelector) {
    $(modalSelector)
        .modal({
            transition: 'horizontal flip',
            autofocus: true
        })
        .modal('attach events', triggerSelector, 'show')
    ;
}

function bindDropdown(selector, apiAction, callbackSuccess) {
    var $element = $(selector);
    var $items = $element.find('.menu');

    if (callbackSuccess == null) {
        callbackSuccess = function (response) {
            $.each(response.data, function (index, v) {
                var $item = $('<div class="item" data-value="' + v.value + '">' + v.name + '</div>');
                if (v.description != null) {
                    var $description = $('<span class="description">' + v.description + '</span>');
                    $item.append($description);
                }

                $items.append($item);
            });
        };
    }

    $element.api({
        on: 'now',
        action: apiAction,
        onSuccess: callbackSuccess
    }).dropdown();
}