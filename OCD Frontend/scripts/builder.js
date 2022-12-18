


class Builder{

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

        algo.forEach(algorithm => {
            
            let checkbox = '';

            checkbox += '<div class="form-check form-switch">';
            checkbox += '   <input class="form-check-input" name="check-algos" type="checkbox" role="switch" id="'+algorithm.id+'">';
            checkbox += '   <label class="form-check-label" for="'+algorithm.id+'">'+algorithm.name+'</label>';
            checkbox += '<div id="emailHelp" class="form-text">'+algorithm.description+'</div>'
            checkbox += '</div>';
            
            wrapper.innerHTML += checkbox;

        });


    }


    BuildCard(algo,result){
        let card = document.getElementById("card-"+algo);

        card.innerHTML = "";

        let inner = "";
        
            inner += '<div>';
                inner += '<div class="card-body">';
                    inner += '<h5 class="card-title mb-3">Algorithm : '+ algo +'</h5>';

                    inner += '<div class="form-floating mb-3">';
                        inner += '<textarea class="form-control" placeholder="Leave a comment here" id="text-'+algo+'" style="height: 30vh">';
                            inner += result.words[0];
                        inner += '</textarea>';
                        inner += '<label for="text-'+algo+'">Words</label>';
                    inner += '</div>';

                    inner += '<input type="button" value="Save Changes" class="btn btn-primary">';
                inner += '</div>';
            inner += '</div>';
        
        card.innerHTML = inner;

    }

    InitCardWrapper(){
        let wrapper = document.getElementById("cards-wrapper")
        wrapper.innerHTML = "";
    }

    InitCard(algo){

        let wrapper = document.getElementById("cards-wrapper")

        let inner = "";
        inner += "<div class='row panel border mb-3' id='card-"+algo+"'>";
        inner += '<div class="text-center">';
        inner += '<div  class="col align-self-center"><div class="lds-dual-ring"></div></div>';
        inner += '</div>';
        inner += "</div>";

        wrapper.innerHTML += inner;
    }

    BuildCardHistory(algo,result){
        let card = document.getElementById("card-"+algo);

        card.innerHTML = "";

        let inner = "";
        
            inner += '<div>';
                inner += '<div class="card-body">';
                    inner += '<h5 class="card-title mb-3">Algorithm : '+ algo +'</h5>';

                    inner += '<div class="form-floating mb-3">';
                        inner += '<textarea class="form-control" placeholder="Leave a comment here" id="text-'+algo+'" style="height: 30vh">';
                            inner += result.words[0];
                        inner += '</textarea>';
                        inner += '<label for="text-'+algo+'">Words</label>';
                    inner += '</div>';

                    inner += '<input type="button" value="Save Changes" class="btn btn-primary">';
                inner += '</div>';
            inner += '</div>';
        
        card.innerHTML = inner;

    }




}
