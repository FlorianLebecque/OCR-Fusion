let nameimg;
let isHandWritten;
let algorithm = [];
let srcimg;

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

document.getElementById("findtext").addEventListener("click", function(event){
    event.preventDefault();
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
        let sessions = "Sarah"
        let body = {
            session : sessions,
            imageName : nameimg,
            isHandWritten : true,
            regions :[[{
                x : 0,
                y : 0
            }]]
        }

        //essaye d'envoyer image a l'API mais fonctionne pas
        const formData = new FormData();
        const fileField = document.getElementById('file-ip-1');

        //formData.append('username', 'abc123');
        formData.append('file', fileField.files[0]);

        fetch('http://127.0.0.1:5154/Ocr', {
        method: 'PUT',
        body: formData
        })
        .then((response) => response.json())
        .then((result) => {
            console.log('Success:', result);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
        //let bodyJSON = JSON.stringify(body);
        console.log("body : " +body);
        postData('http://127.0.0.1:5154/Ocr', body)
        .then((data) => {
        console.log(data); // JSON data parsed by `data.json()` call
        });
        ShowResults();
        // fetch('http://127.0.0.1:5154/Ocr', { mode: 'no-cors'},{
        // method: 'POST', // or 'PUT'
        // headers: {
        //     'Content-Type': 'application/json',
        // },
        // bodyJSON: JSON.stringify(body),
        // })
        // .then((response) => response.json())
        // .then((data) => {
        //     console.log('Success:', data);
        // })
        // .catch((error) => {
        //     console.error('Error********:', error);
        // });
        //alert(bodyJSON);//envoie el body à l'API
        document.getElementById("firstpreview").style.display = "none";}
});

//function EnvoiePost()

function ShowResults(){
    document.getElementById("preview-results").style.display = "block";
    var preview = document.getElementById("display-result-img");
    preview.src = srcimg;
    preview.style.display = "block";
    document.getElementById("preview-is-handwritten").style.display = "block";
    document.getElementById("preview-is-printed").style.display = "block";
    document.getElementById("preview-is-other").style.display = "block";
    //return false;
}

async function postData(url = '', data = {}) {//fonctionne pas
    // Default options are marked with *
    const response = await fetch(url, {
      method: 'POST', // *GET, POST, PUT, DELETE, etc.
      mode: 'cors', // no-cors, *cors, same-origin
      cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
      credentials: 'same-origin', // include, *same-origin, omit
      headers: {
        'Content-Type': 'application/json'
        // 'Content-Type': 'application/x-www-form-urlencoded',
      },
      redirect: 'follow', // manual, *follow, error
      referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
      body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    console.log("data : " +data);
    return response.json(); // parses JSON response into native JavaScript objects
  }
  
  
