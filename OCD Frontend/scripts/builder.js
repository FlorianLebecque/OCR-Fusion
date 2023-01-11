


class Builder{

    constructor(_paramertersbuilder) {
        this.parametersBuilder = _paramertersbuilder;
    }

    async Load(){

        await fetch('http://127.0.0.1:5154/Algorithms')
            .then((response) => response.json())
            .then((json) => {
                this.BuildCheckBox(json);
            });
    }

    BuildCheckBox(algo){
        this.algo = algo;

        let wrapper = document.getElementById("checkbox-wrapper");

        let count = 0;

        algo.forEach(algorithm => {
            
            let checkbox = '';

            if(count == (algo.length -1)){
                checkbox += '<div style="display:flex; justify-content:space-between;align-items:center;">';

            }else{
                checkbox += '<div class="mb-3" style="display:flex;justify-content:space-between;align-items:center;">';
            }

            checkbox += '<div class="form-check form-switch me-2">';
            checkbox += '   <input class="form-check-input" name="check-algos" type="checkbox" role="switch" id="'+algorithm.id+'" data-bs-toggle="collapse" data-bs-target="#param-'+algorithm.id+'" aria-expanded="false" aria-controls="param-'+algorithm.id+'">';
            checkbox += '   <label class="form-check-label" for="'+algorithm.id+'">'+algorithm.name+'</label>';
            checkbox += '</div>';
            checkbox += '<div class="form-text">'+algorithm.description+'</div>'

            checkbox += '</div>';

            if(Object.keys(algorithm.parameters).length !== 0){
                checkbox += '<div class="collapse mt-3 mb-3" id="param-'+algorithm.id+'">';
                checkbox += '   <div class="card card-body">';
                checkbox += this.parametersBuilder.Build(algorithm.id,algorithm.parameters);
                checkbox += '   </div>';
                checkbox += '</div>';
            }
            
            count++;

            wrapper.innerHTML += checkbox;
        });


    }

    GetAlgoName(algo_id){
        

        for(let index in this.algo){
            if(this.algo[index].id == algo_id){
                return this.algo[index].name;
            }
        }

    }

    BuildCard(algo,result){
        let card = document.getElementById("card-"+algo);
        card.innerHTML = "";

        let inner = "";
        
        
        inner += '  <div class="t-card">';
            inner += '<h2>'+ this.GetAlgoName(algo) +'</h2>';
                        inner += '<textarea id="text-'+algo+'">';
                            inner += result.words[0];
                        inner += '</textarea>';
                    inner += '<button class="btn btn-success"><i class="display-6 bi bi-check2"></i></button>';
                inner += '</div>';
            inner += '</div>';
        
        card.innerHTML = inner;

    }

    InitCardWrapper(){
        let wrapper = document.getElementById("cards-wrapper")
        wrapper.innerHTML = "";
    }

    InitCard(algo,imageName){

        let wrapper = document.getElementById("cards-wrapper")

        let inner = "";
        inner += "<div class='card-panel' id='card-"+algo+"' onclick='select(\"card-"+algo+"\")'>";
        inner += '  <div class="t-card">';
        inner += '      <div  class="col align-self-center"><div class="lds-dual-ring"></div></div>';
        inner += '  </div>';
        inner += "</div>";

        wrapper.innerHTML += inner;

        let card = document.getElementById("card-"+algo);

        let urlimage = 'http://127.0.0.1:5154/Image/'+imageName;
        //card.style.backgroundImage = "url('"+urlimage+"')";

    }

    BuildCardHistory(algo,result){
        let imageHold = document.getElementById("imageHolder");
        //imageHold.innerHTML =`<img src="http://127.0.0.1:5154/Image/${result.imageName}" style="height:auto; width:100%;"class="img-fluid" id="imageHolder1" alt="">`;
        imageHold.innerHTML =`<img src="http://127.0.0.1:5154/Image/${result.imageName}" style="height:auto; width:40vw;"class="img-fluid" id="imageHolder1" alt="">`;



        let card = document.getElementById("card-"+algo);
        let urlimage = 'http://127.0.0.1:5154/Image/'+result.imageName;

        card.innerHTML = "";

        let inner = "";
            inner += '<div class="t-card">';
            inner += '<h3>Image Name : <br>'+ result.imageName +'</h3>';
            inner += '<h3>Algorithm : '+ algo +'</h3>';
                    inner += '<textarea id="text-'+algo+'" rows="400" cols="40">';
                            inner += result.words[0];
                        inner += '</textarea>';
                    inner += '<button class="btn btn-success" id="btnsavemodif"><i class="display-6 bi bi-check2"></i></button>';
                inner += '</div>';
            inner += '</div>';


        
        card.innerHTML = inner;

    }




}
