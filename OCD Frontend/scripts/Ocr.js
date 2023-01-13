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
                        $("#loader").hide();
                        ocr.filename = image.responseText;
                        
                        setTimeout(()=>{
                            GetyImg('http://127.0.0.1:5154/Image/'+image.responseText);
                        },150);
                    }
                }
                
            });
        

        // this.filename = files[0].name;
        // let filename = this.filename;
        // data.append('file', files[0]);      

        // imageWrapper.show(100);

        
        // $.ajax({
        //     url: "http://127.0.0.1:5154/Ocr",
        //     type: 'put',
        //     dataType: 'json',
        //     data: data,
        //     processData: false,
        //     contentType: false,
        //     statusCode:{
        //         200: function(image){
        //             $("#loader").hide();
        //             setTimeout(()=>{
        //                 GetyImg('http://127.0.0.1:5154/Image/'+filename);

        //             },150);
        //         }
        //     }
        // });
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

        let img = {
            width : Utils.elementWidth(p5Div),
            height : Utils.elementHeight(p5Div)
        };
        let wrapper = document.getElementById("result-wrapper");
        let inner = ""
        wrapper.innerHTML = "";
        if (img.width < img.height){
            inner += '<div class="row">'
            inner += '<div class="col-md-4 col-xs-12">'
            inner += '  <img src="http://127.0.0.1:5154/Image/'+this.filename+'" style="border-radius:0.5em;height:100%; width:auto;" class="img-fluid" id="imageHolder" alt="">'
            inner += '</div>'
            inner += '<div class="col-md-8 col-xs-12">'
            inner += '  <div id="cards-wrapper"></div>'
            inner += '</div>'
            inner += '</div>'
            this.imgFormat = 'portrait';
        }
        else {
            inner += '<div class="col-xl-12 mb-3">'
            inner += '  <img src="http://127.0.0.1:5154/Image/'+this.filename+'" style="border-radius:0.5em;height:auto; width:100%;"class="img-fluid" id="imageHolder" alt="">'
            inner += '</div>'
            inner += '<div class="col-xl-12">'
            inner += '  <div id="cards-wrapper"></div>'
            inner += '</div>'
            this.imgFormat = 'paysage';
        }
        wrapper.innerHTML = inner;

        $("#control").hide();

        this.builder.InitCardWrapper();

        algos.forEach((check)=>{

            if(check.checked == false){
                return;
            }


            let ocr_algo = check.id;
            let parameters_obj = {};

            let parameters_inputs = document.getElementsByName(ocr_algo);

            parameters_inputs.forEach((input) => {

                let key = input.id.split('_')[1];
                let val = input.value;

                if(input.type == "checkbox"){
                    val = str(input.checked);
                }


                parameters_obj[key] = val;

            });

            this.builder.InitCard(ocr_algo,this.filename);

            let selected_regions = [];
            if((p1n)&&(p2n)){                
                                
                selected_regions = [[p1n,p2n]];
            }

            let payload = {
                session : session_name,
                imageName : this.filename,
                algo : ocr_algo,
                parameters : parameters_obj,
                regions : selected_regions
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

    DisplayTableRowHistory(json, start_index, end_index){

        $("#itemShow tr").remove(); 
        let tab_start = start_index - 1;
        let tab_end = end_index;

        for(let i = tab_start; i<tab_end; i++){
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
            button.className = "form-control btn btn-success";
            button.style.backgroundColor = "#CF7041";
            button.style.border = "0px";
            tdPreview.className="text-truncate";
            tdPreview.style.maxWidth="350px";

            button.addEventListener("click", () => {
                console.log(data);

                this.builder.InitCardWrapper();
                this.builder.InitCard(data.algorithm, data.imageName);
                this.builder.BuildCardHistory(data.algorithm,data);
                event.preventDefault();//garder le event sinon fonctionne pas !!! 
            });
            tdView.appendChild(button);
            tr.appendChild(tdImageName);
            tr.appendChild(tdPreview);
            tr.appendChild(tdAlgo);
            tr.appendChild(tdView);
            document.getElementById("itemShow").appendChild(tr);
        }


    };

    BrowseHistory(){
        //pour pagination 
        let current_index = 1;//commence par la première page
        let start_index = 1;
        let end_index = 0;
        document.getElementById("table-warper").style.display = "block"

        //initialise session

        let url = new URL('http://127.0.0.1:5154/Ocr');
        document.getElementById("itemShow").innerHTML = "";

        let session_name = (document.getElementById("session_name").value == "")? "default": document.getElementById("session_name").value;
        let params = {session:session_name};
        url.search = new URLSearchParams(params).toString();

        //appel API
        fetch(url)
        .then((response) => response.json())// Converting received data to JSON
        .then((json) => {
            
            //initialise pagination
            let table_size = parseInt(document.getElementById("table_size").value); //used to display the number of table rows. Default value is 10
            let array_lenght = json.length;
            let max_index = parseInt(array_lenght / table_size);
            if(array_lenght % table_size > 0){
                max_index++;}
            
            //paramètre les bouton previous next
            $(".index_buttons button").remove();
            $(".index_buttons").append('<button id="prevbut">Previous</button>');
            const prevbut = document.getElementById("prevbut");
            prevbut.addEventListener("click", ()=>{
                if (current_index > 1){
                    current_index--;
                    //trouver moyen d'en faire une focntion
                    start_index = ((current_index - 1) * table_size)+1;
                    end_index = (start_index + table_size) -1;
                    if(end_index > array_lenght){
                        end_index = array_lenght;
                    }
                    $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
                    $(".index_buttons button").removeClass('active');
                    $(".index_buttons button[index='"+current_index+"']").addClass('active');
            
                    ocr.DisplayTableRowHistory(json, start_index, end_index);}
            });

            for(var i=1; i< max_index+1; i++){
                $(".index_buttons").append('<button class="numbut" id="numbut'+i+'" index="'+i+'">'+i+'</button>');
                console.log("numbut"+i)
                document.getElementById("numbut"+i).addEventListener("click", function(e){
                    var target = e.target;
                    var parent = target.parentNode;
                    var index = [].indexOf.call(parent.children, target);
                    console.log("index:", index);
                    current_index = index;
                    start_index = ((current_index - 1) * table_size)+1;
                    end_index = (start_index + table_size) -1;
                    if(end_index > array_lenght){
                        end_index = array_lenght;
                    }
                    $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
                    $(".index_buttons button").removeClass('active');
                    $(".index_buttons button[index='"+current_index+"']").addClass('active');
            
                    ocr.DisplayTableRowHistory(json, start_index, end_index);
                })
            }


            $(".index_buttons").append('<button id="nextbut">Next</button>');
            const nextbut = document.getElementById("nextbut");
            nextbut.addEventListener("click", function(){
                if (current_index < max_index){
                    current_index++;
                    //trouver moyen d'en faire une focntion
                    start_index = ((current_index - 1) * table_size)+1;
                    end_index = (start_index + table_size) -1;
                    if(end_index > array_lenght){
                        end_index = array_lenght;
                    }
                    $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
                    $(".index_buttons button").removeClass('active');
                    $(".index_buttons button[index='"+current_index+"']").addClass('active');
            
                    ocr.DisplayTableRowHistory(json, start_index, end_index);}
            });

            start_index = ((current_index - 1) * table_size)+1;
            end_index = (start_index + table_size) -1;
            if(end_index > array_lenght){
                end_index = array_lenght;
            }
            $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
            $(".index_buttons button").removeClass('active');
            $(".index_buttons button[index='"+current_index+"']").addClass('active');

            this.DisplayTableRowHistory(json, start_index, end_index);

            const selectsize = document.getElementById("table_size");
            selectsize.addEventListener("change", function (){
                table_size = parseInt($(this).val());
                current_index = 1;
                start_index = 1;
                end_index = (start_index + table_size) -1;
                if(end_index > array_lenght){
                    end_index = array_lenght;
                }
                max_index = parseInt(array_lenght / table_size);
                if(array_lenght % table_size > 0){
                    max_index++;}
                $(".index_buttons button").remove();
                $(".index_buttons").append('<button id="prevbut">Previous</button>');
                const prevbut = document.getElementById("prevbut");
                prevbut.addEventListener("click", ()=>{
                    if (current_index > 1){
                        current_index--;
                        //trouver moyen d'en faire une focntion
                        start_index = ((current_index - 1) * table_size)+1;
                        end_index = (start_index + table_size) -1;
                        if(end_index > array_lenght){
                            end_index = array_lenght;
                        }
                        $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
                        $(".index_buttons button").removeClass('active');
                        $(".index_buttons button[index='"+current_index+"']").addClass('active');
                
                        ocr.DisplayTableRowHistory(json, start_index, end_index);}
                });
                for(var i=1; i< max_index+1; i++){
                    $(".index_buttons").append('<button class="numbut" id="numbut'+i+'" index="'+i+'">'+i+'</button>');
                    console.log("numbut"+i)
                    document.getElementById("numbut"+i).addEventListener("click", function(e){
                        var target = e.target;
                        var parent = target.parentNode;
                        var index = [].indexOf.call(parent.children, target);
                        console.log("index:", index);
                        current_index = index;
                        start_index = ((current_index - 1) * table_size)+1;
                        end_index = (start_index + table_size) -1;
                        if(end_index > array_lenght){
                            end_index = array_lenght;
                        }
                        $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
                        $(".index_buttons button").removeClass('active');
                        $(".index_buttons button[index='"+current_index+"']").addClass('active');
                
                        ocr.DisplayTableRowHistory(json, start_index, end_index);
                    })
                }

                $(".index_buttons").append('<button id="nextbut">Next</button>');
                const nextbut = document.getElementById("nextbut");
                nextbut.addEventListener("click", function(){
                    if (current_index < max_index){
                        current_index++;
                        //trouver moyen d'en faire une focntion
                        start_index = ((current_index - 1) * table_size)+1;
                        end_index = (start_index + table_size) -1;
                        if(end_index > array_lenght){
                            end_index = array_lenght;
                        }
                        $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
                        $(".index_buttons button").removeClass('active');
                        $(".index_buttons button[index='"+current_index+"']").addClass('active');
                
                        ocr.DisplayTableRowHistory(json, start_index, end_index);}
                });
                $(".footer span").text('Showing '+start_index+' to '+end_index+' of '+array_lenght+' entries');
                $(".index_buttons button").removeClass('active');
                $(".index_buttons button[index='"+current_index+"']").addClass('active');
                ocr.DisplayTableRowHistory(json, start_index, end_index);
                });
        });

    };

    Update(id,text_area_id){


        let update_definition = {
            Id:id,
            words: document.getElementById(text_area_id).value
        }

        console.log(update_definition);

        fetch('http://127.0.0.1:5154/Ocr', {
                method: 'PATCH', // or 'PUT'
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(update_definition),
            })
            .then((response) => response.json())
            .then((data) => {
                //alert("Updated");
            })
            .catch((error) => {
                //alert("Error");
            });

    };

    //Download(json, imageName){
    Download(areaid, img){
        let text = document.getElementById(areaid).value;
        let data = "text/json;charset=utf-8," + text;
        let a = document.createElement('a');
        a.href = 'data:' + data;
        a.download = img +'.txt';
        a.innerHTML = 'download JSON';
        a.click();
    };
}