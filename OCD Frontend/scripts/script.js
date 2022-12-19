let nameimg;
let isHandWritten;
let algorithm = [];
let srcimg;


$("#imageWrapper").hide();
$("#camera-wrapper").hide();

let parametersBuilder = new Paramertersbuilder();

let builder = new Builder(parametersBuilder);
builder.Load();

let ocr = new OcrAPI(builder);








/*

function showPreview(event, iduplaod){ 
    if(event.target.files.length > 0){
        var img = document.getElementById(iduplaod);//prend le nom du premier,faire que prend le nom du deuxieme
        if (img.files.item(0).type =="application/pdf"){
            alert('Convertir en jpeg png  on senfout');
        }
      nameimg = img.files.item(0).name;
      var src = URL.createObjectURL(event.target.files[0]);
      srcimg = src;
      document.getElementById("firstpreview").style.display = "block";
      var preview = document.getElementById("file-ip-1-preview");
      preview.src = src;
      preview.style.display = "block";
    }
}


document.getElementById('findtext').onclick = function() {
    isHandWritten = document.getElementById('handwritten').checked;
    isPrinted = document.getElementById('printed').checked;
    isOther = document.getElementById('other').checked;
    if (nameimg =="" || nameimg == null){
        alert("Attention, n'oubliez pas de charger une image! ");
    }
    else if (!isHandWritten && !isPrinted && !isOther){
        alert("Select an algorithm ");
    }
    else {
        if (isHandWritten){
            algorithm.push("handwritten")
        }
        if (isPrinted){
            algorithm.push("printed")
        }
        if (isOther){
            algorithm.push("other")
        }
        let body = {
            imageName : nameimg,
            algorithm : algorithm,
            regions :[]
        }
        let bodyJSON = JSON.stringify(body);
        ShowResults();
        alert(bodyJSON);//envoie el body à l'API
        //document.getElementById("firstpreview").style.display = "none";
        document.getElementById("recherche").innerHTML = nameimg + algorithm;//fonctionne pas
    }
    return false;

}

//function EnvoiePost()

function ShowResults(){
    document.getElementById("preview-results").style.display = "block";
    var preview = document.getElementById("display-result-img");
    preview.src = srcimg;
    preview.style.display = "block";
    document.getElementById("preview-is-handwritten").style.display = "block";
    document.getElementById("preview-is-printed").style.display = "block";
    document.getElementById("preview-is-other").style.display = "block";
    return false;
}
*/