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
        this.filename = filename
        let blobBin = atob(canvas.toDataURL('image/jpeg').split(',')[1]);
        let array = [];
        for(let i = 0; i < blobBin.length; i++) {
            array.push(blobBin.charCodeAt(i));
        }
        let file = new Blob([new Uint8Array(array)], {type: 'image/jpeg'});
        file = new File([file], filename)

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
        let session_name = (document.getElementById("session_name").value == "")? "default": document.getElementById("session_name").value;

        let current_index = 1;//commence par la première page
        let start_index = 1;
        let end_index = 10;

        let url = new URL('http://127.0.0.1:5154/Ocr');
        document.getElementById("itemShow").innerHTML = "";

        let params = {session:session_name};
        url.search = new URLSearchParams(params).toString();
        fetch(url)
        // Converting received data to JSON
        .then((response) => response.json())
        .then((json) => {
            let table_size = 10;
            let array_lenght = json.length;
            let max_index = array_lenght / table_size;
            if(array_lenght % table_size > 0){
                max_index++;
            }
            
        // 2. Create a variable to store HTML table headers

            // 3. Loop through each data and add a table row//https://getbootstrap.com/docs/5.0/helpers/text-truncation/
            //json.forEach((data) => {
            for (let i = 0; i<array_lenght; i++){
                let data = json[i];

                var tr = document.createElement("tr");
                var tdImageName = document.createElement("td");
                var tdPreview = document.createElement("td");
                var tdAlgo = document.createElement("td");
                var tdView = document.createElement("td");
                var button = document.createElement("button");
                tdImageName.innerHTML = data.imageName;
                tdPreview.innerHTML = data.words;
                tdAlgo.innerHTML = data.algorithm;
                button.innerText = "View";
                tdPreview.className="d-inline-block text-truncate";
                tdPreview.style.maxWidth="350px";

                button.addEventListener("click", () => {

                    this.builder.InitCardWrapper();
                    this.builder.InitCard(data.algorithm);
                    this.builder.BuildCardHistory(data.algorithm,data);
                    event.preventDefault();//garder le event sinon fonctionne pas !!! 
                });
                tdView.appendChild(button);
                tr.appendChild(tdImageName);
                tr.appendChild(tdPreview);
                tr.appendChild(tdAlgo);
                tr.appendChild(tdView);
                document.getElementById("itemShow").appendChild(tr);
                //Regarder pour le style https://www.youtube.com/watch?v=EsB0ufgLytk&list=PLyb-PdAs945lKNTwnXzmP58kWhk_0Xteg&index=1&t=0s

            };
        
        $(".index_buttons button").remove();
        $(".index_buttons").append('<button>Previous</button>');
        for(var i=1; i<+ max_index; i++){
            $(".index_buttons").append('<button index="'+i+'">'+i+'</button>');
        }
        $(".index_buttons").append('<button>Next</button>');
        });
        //Finir tuto = https://www.youtube.com/watch?v=xKjz4mv77Ls&list=PLyb-PdAs945lKNTwnXzmP58kWhk_0Xteg&index=1&t=0s

    };

}