let nameimg;
let isHandWrittenVal;

function showPreview(event){ 
    if(event.target.files.length > 0){
        var img = document.getElementById('file-ip-1');
        if (img.files.item(0).type =="application/pdf"){
            alert('Convertir en jpeg png  on senfout');
        }
      nameimg = img.files.item(0).name;
      var src = URL.createObjectURL(event.target.files[0]);
      var preview = document.getElementById("file-ip-1-preview");
      preview.src = src;
      preview.style.display = "block";
      document.getElementById("recherche").innerHTML = nameimg + isHandWrittenVal;
    }
  }

document.getElementById('type').onchange = function(){
    isHandWrittenVal = document.getElementById('type').checked;
    document.getElementById("recherche").innerHTML = nameimg + isHandWrittenVal;
}

document.getElementById('submit').onclick = function() {
    isHandWrittenVal = document.getElementById('type').checked;
    if (nameimg =="" || nameimg == null){
        alert("Attention, n'oubliez pas de charger une image! ");
    }
    else{
        let body = {
            imageName : nameimg,
            isHandWritten : isHandWrittenVal,
            regions :[]
        }
        let bodyJSON = JSON.stringify(body);
        alert(bodyJSON);
    }
    //let isHandWritten = document.getElementById('type').checked;
    document.getElementById("recherche").innerHTML = nameimg + isHandWrittenVal;
    //si true alors c'est manuscrit et bleu foncé
    //si false c'est que typographie et bleu clair
    //document.getElementById('type').value pour voir la valeur

}

//function EnvoiePost()