var selected = new Boolean();

function auto_select_item() 
{
            selected = false;
            if (selected_file_name != '') {
                FindAndSelectRow();
            }

            if (selected == false || selected_file_name == '') {
                SelectTD($("#GridView1 tr:odd").first());
            }

            $('.site_button').each(function (index, Element) {
                $(Element).mouseover(function () {
                    $(Element).removeClass("site_button");
                    $(Element).addClass("site_button_mouse_over");
                });
                $(Element).mouseout(function () {
                    $(Element).removeClass("site_button_mouse_over");
                    $(Element).addClass("site_button");
                });
            });

            $("#GridViewPanel tr:has(td)").hover(function () {
                $(this).css("cursor", "pointer");
            });

            $(function () {
                $("#GridViewPanel tr:has(td)").click(function () {
                    SelectTD(this);
                });
            });
} //auto_select_item()

function SelectTD(tdElement) {
            $('#GridViewPanel tr').removeClass('selected_table_item');
            $(tdElement).addClass('selected_table_item');
            selected_file_name = $(tdElement).find("td:first-child").text().replace(/\s/g, "");
            var imageUrl = 'ImageProductor.aspx?fileName=' + selected_file_name;
            $('#Image1').attr('src', imageUrl);
        }

function FindAndSelectRow() {
            $('#GridView1 tr').each(
                function () {
                    if ($(this).find("td:first-child").text().replace(/\s/g, "") == selected_file_name) {
                        var td = $(this);
                        SelectTD(td);
                        selected = true;
                    }
                }
                );
} //FindAndSelectRow()