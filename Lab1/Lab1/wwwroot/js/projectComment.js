function loadComments(projectID) {
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectID=' + projectID,
        method: 'GET',
        success: function (data) {
            var commentHtml = '';
            data.forEach(function (comment) {
                commentHtml += `
                <div class="card mb-3">
                    <div class="card-header text-muted">
                        Posted on ${new Date(comment.datePosted).toLocaleString()}
                    </div>
                    <div class="card-body">
                        <p class="card-text">${comment.content}</p>
                    </div>
                </div>`;
            });
            $('#commentsList').html(commentHtml);
        }
    });
}


$(document).ready(function () {
    var projectID = $('#projectComments input[name="ProjectID"]').val();

    loadComments(projectID);

    $('#addCommentForm').submit(function (e) {
        e.preventDefault();

        var formData = {
            ProjectID: projectID,
            Content: $('#projectComments textarea[name="Content"]').val()
        };


        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),

            success: function (response) {
                if (response.success) {
                    $('#projectComments textarea[name="Content"]').val(''); //Clear message area
                    loadComments(projectID);

                } else {
                    alert(response.message);

                }

            },
            error: function (xhr, status, error) {
                alert("Error " + error);

            }

        });

    });

});
