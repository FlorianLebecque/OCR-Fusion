class OcrAPI{

    constructor(_builder) {
        this.builder = _builder;
    }

    Upload()
    {

        let imageWrapper = $("#imageWrapper");

        let data = new FormData(),
            files = document.getElementById("imageUpload").files;

        this.filename = files[0].name;
        let filename = this.filename;
        data.append('file', files[0]);

        imageWrapper.html('<div class="col align-self-center"><div class="lds-dual-ring"></div></div>');
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

    async OpenCamera(){


        let video = document.querySelector("#video");
        let main = $("#main-wrapper");
        let camera = $("#camera-wrapper");


        main.hide();
        camera.show();
        let stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false, facingMode: 'environement' });
	    video.srcObject = stream;

    }

    TakePhoto(){
        let canvas = document.querySelector("#canvas");
        let video = document.querySelector("#video");
        let imageWrapper = $("#imageWrapper");
        let main = $("#main-wrapper");
        let camera = $("#camera-wrapper");


        canvas.width = video.offsetWidth;
        canvas.height = video.offsetHeight;

        canvas.getContext('2d').drawImage(video, 0, 0, video.offsetWidth, video.offsetHeight);



        let filename = "test.jpeg";
        
        var blobBin = atob(canvas.toDataURL('image/jpeg').split(',')[1]);
        var array = [];
        for(var i = 0; i < blobBin.length; i++) {
            array.push(blobBin.charCodeAt(i));
        }
        var file=new Blob([new Uint8Array(array)], {type: 'image/png'});


        console.log(file);

        let data_file = new FormData();
        data_file.append("file",file);

        main.show();
        camera.hide();
        imageWrapper.html('<div  class="col align-self-center"><div class="lds-dual-ring"></div></div>');
        imageWrapper.show(100);

        $.ajax({
            url: "http://127.0.0.1:5154/Ocr",
            type: 'put',
            dataType: 'json',
            data: data_file,
            processData: false,
            contentType: false,
            statusCode:{
                200: function(image){

                    imageWrapper.html('<img src="http://127.0.0.1:5154/Image/'+filename+'" id="imageHolder" alt="">');
                }
            }
            
        });
        
    }


    RequestOCR(){

        let algos = document.getElementsByName("check-algos");
        let session_name = (document.getElementById("session").value == "")? "default": document.getElementById("session").value;

        this.builder.InitCardWrapper();

        algos.forEach((check)=>{

            if(check.checked == false){
                return;
            }


            let ocr_algo = check.id;

            this.builder.InitCard(ocr_algo);


            let payload = {
                session : session_name,
                imageName : this.filename,
                algo : ocr_algo,
                regions : []
            }
            
            fetch('http://127.0.0.1:5154/Ocr', {
                method: 'POST', // or 'PUT'
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(payload),
            })
            .then((response) => response.json())
            .then((data) => {
                this.builder.BuildCard(ocr_algo,data);
            })
            .catch((error) => {
                //display error
            });
        });

    }

    BrowseHistory(){
        //let session_name = "test";
        let session_name = (document.getElementById("session_name").value == "")? "default": document.getElementById("session_name").value;

        //let session_name = (document.getElementById("session_name").value);
        let payload = {
            session : session_name
        };
        let url = new URL('http://127.0.0.1:5154/Ocr');

        let params = {session:session_name};
        url.search = new URLSearchParams(params).toString();
        fetch(url)
        // Converting received data to JSON
        .then((response) => response.json())
        .then((json) => {
            
        // 2. Create a variable to store HTML table headers
            let li = `<tr><th>Image name</th><th>Preview of the result</th><th>Algorithm</th><th>View</th></tr>`;
        
            // 3. Loop through each data and add a table row//https://getbootstrap.com/docs/5.0/helpers/text-truncation/
            json.forEach((user) => {
            li += `<tr>
                <td>${user.imageName}</td>
                <td class="d-inline-block text-truncate" style="max-width: 350px;">${user.words} </td>
                <td class="d-inline-block text-truncate" style="max-width: 150px;">Algo</td>
                <td class="d-inline-block text-truncate" style="max-width: 150px;">View</td>
            </tr>`;
            });
            //<td>${user.words} </td>
        
            // 4. DOM Display result
            document.getElementById("table_result").innerHTML = li;
        });

    }

}