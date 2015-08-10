$(function () {
    $('.date').datepicker({
        autoclose: true
    });

    $('#customer-list').tablesorter({
        sortList: [[0, 0]],
        headers: { 8: { sorter: false }, 9: { sorter: false }, 10: { sorter: false } }
    });

    $('.ddlCity').change(function () {
        $.get('/Home/GetRegionsInCity/' + $(this).val(), function (response) {
            var regions = $.parseJSON(response);
            var ddlCityRegions = $('.ddlCityRegion');

            $('.ddlCityRegion > option').remove();

            for (var i = 0; i < regions.length; i++) {
                ddlCityRegions.append($('<option />').val(regions[i].Value).text(regions[i].Text));
            }
        });
    });

    if ($('#loginForm .validation-summary-errors').length) {
        $('.form-group .col-md-10').addClass('has-error');
    }
});