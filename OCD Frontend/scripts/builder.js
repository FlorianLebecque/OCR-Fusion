


class Builder{

    constructor(){

        
    }

    async Load(){

        var opts = {
            //mode:'no-cors'
        }

        await fetch('http://127.0.0.1:5154/Algorithme',opts)
            .then((response) => response.json())
            .then((json) => {
                
                this.BuildCheckBox(json);
            });
    }

    BuildCheckBox(algo){

        console.log(algo)

        let wrapper = document.getElementById("checkbox-wrapper");

        algo.forEach(algorithm => {
            
            let checkbox = '';

            checkbox += '<div class="form-check form-switch">';
            checkbox += '   <input class="form-check-input" type="checkbox" role="switch" id="'+algorithm.id+'">';
            checkbox += '   <label class="form-check-label" for="'+algorithm.id+'">'+algorithm.name+'</label>';
            checkbox += '<div id="emailHelp" class="form-text">'+algorithm.description+'</div>'
            checkbox += '</div>';
            
            wrapper.innerHTML += checkbox;

        });


    }


}
