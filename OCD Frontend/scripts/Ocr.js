class OcrAPI{

    Upload()
    {

        let imageWrapper = $("#imageWrapper");

        let data = new FormData(),
            files = document.getElementById("imageUpload").files;

        this.filename = files[0].name;
        let filename = this.filename;
        data.append('file', files[0]);

        imageWrapper.html('<div  class="col align-self-center"><div class="lds-dual-ring"></div></div>');
        imageWrapper.show(100);

        
        $.ajax({
            url: "http://127.0.0.1:5154/Ocr",
            type: 'put',
            dataType: 'json',
            data: data,
            processData: false,
            contentType: false,
            statusCode:{
                200: function(image){
                    imageWrapper.html('<img src="http://127.0.0.1:5154/Image/'+filename+'" id="imageHolder" alt="">');
                }
            }
            
        });
    };

}