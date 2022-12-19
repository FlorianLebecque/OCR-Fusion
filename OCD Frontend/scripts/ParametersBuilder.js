class Paramertersbuilder{
    Build(algo_id,parameters){

        let form = "";

        for (const key in parameters) {

            form += this.CreateControl(algo_id,algo_id+"_"+key,parameters[key]);

        }

        return form;
    }


    CreateControl(algo_id,id,parameter){

        switch(parameter.type){
            case "text":
                return this.CreateText(algo_id,id,parameter);
            
            case "check":
                return this.CreateCheck(algo_id,id,parameter);

            case "select":
                return this.CreateSelect(algo_id,id,parameter);

        }


    }

    CreateCheck(algo_id ,id,textParameter){

        let textParam = "";

        textParam += '<div class="mb-3 parameters-holder">';
        textParam += '  <div class="form-check form-switch me-2">';
        textParam += '      <label class="form-check-label" for="'+id+'">'+textParameter.title+'</label>';
        textParam += '      <input class="form-check-input" name="'+algo_id+'" type="checkbox" role="switch" id="'+id+'" >';
        textParam += '  </div>';
        textParam += '  <div class="form-text">'+textParameter.description+'</div>'
        textParam += '</div>';

        return textParam;

    }

    CreateSelect(algo_id ,id,textParameter){

        let textParam = "";

        textParam += '<div class="mb-3 parameters-holder" style="display:flex;">';

        textParam += '<label for="'+id+'" class="form-label">'+textParameter.title+'</label>';

        textParam += '<select class="form-select" aria-label="Default select example">';

        let options = JSON.parse(textParameter.options);
        console.log(options);

        for(const val in options){

            if(val == textParameter.default){
                textParam += '  <option value="'+val+' selected>'+options[val]+'</option>';
            }else{
                textParam += '  <option value="'+val+'">'+options[val]+'</option>';
            }

        }
        textParam += '</select>';
        textParam += '<div class="form-text">'+textParameter.description+'</div>'


        textParam += '</div>';





        return textParam;

    }

    CreateText(algo_id ,id,textParameter){

        let textParam = "";

        textParam += '<div class="mb-3 parameters-holder">';
        textParam += '<label for="'+id+'" class="form-label">'+textParameter.title+'</label>';
        textParam += '<input name="'+algo_id+'" type="text" class="form-control" id="'+id+'" value="'+textParameter.default+'" >';
        textParam += '<div class="form-text">'+textParameter.description+'</div>'

        textParam += '</div>';


        return textParam;

    }

}