$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);

        const customAlert = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-danger mx-2",
                cancelButton: "btn btn-success"
            },
            buttonsStyling: false
        });


        customAlert.fire({
            title: "Are you sure that you need to delete this game?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            cancelButtonText: "No, cancel!",
            reverseButtons: true
        }).then((result) => {
            console.log(result.isConfirmed);
            if (result.isConfirmed) {
                $.ajax({
                    url: `/Games/Delete/${btn.data('id')}`,
                    method: 'DELETE',
                    success: function () {
                        customAlert.fire({
                            title: "Deleted!",
                            text: "Game has been deleted.",
                            icon: "success"
                        });
                        btn.parents('tr').fadeOut();
                    }, error: function () {
                        customAlert.fire({
                            title: "Oooops...!",
                            text: "Something went wrong!",
                            icon: "error"
                        });
                    }
                });
               
            }
        });


    })
})