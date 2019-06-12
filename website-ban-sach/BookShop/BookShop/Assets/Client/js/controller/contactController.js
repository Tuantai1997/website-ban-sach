var contact = {
    init: function () {
        contact.registerEvents();
        
    },
    registerEvents: function () {
       
        $('#btnSend')/*.off('click')*/.on('click', function () {           
            //var data = {
            var name= $('#txtName').val();
            var mobile= $('#txtMobile').val();
            var address= $('#txtAddress').val();
            var email= $('#txtEmail').val();
            var content= $('#txtContent').val();
            //};
            $.ajax({
                url: '/Contact/Send',
                type: 'POST',
                dataType: 'json',
                async: true,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    name: name,
                    mobile: mobile,
                    address: address,
                    email: email,
                    content: content
                }
                ),
                success: function (res) {
                    if (res.status === true) {
                        window.alert('Gửi thành công',"success");
                        contact.resetForm();
                    }
                }
            });
        });
    },
    resetForm: function () {
        $('#txtName').val('');
        $('#txtMobile').val('');
        $('#txtAddress').val('');
        $('#txtEmail').val('');
        $('#txtContent').val('');
    }
};
contact.init();