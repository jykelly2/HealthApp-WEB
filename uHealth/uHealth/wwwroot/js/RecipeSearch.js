

document.addEventListener('DOMContentLoaded', (event) => {

    $("#recipe_search").click(function () {

        var searchText = $("#searchText").val();
        var baseURL = "https://api.edamam.com/search?app_id=3a146f47&app_key=9ca8209de9cd6352ef0d03bad10ecfc8"

        var finalURL = baseURL + "&q=" + searchText + "&from=1&to=7&ingr=5"


        $.get(finalURL,
            function (data) {

                let recipes = data;
                let output = '';
                console.log(recipes);

                for (let i = 0; i < 6; i++) {

                    output += createItem(recipes["hits"][i]["recipe"]);

                }
                $("#recipes").html(output);
            });


    });



});


function createItem(obj) {

    var ingrediants = '';
    for (let i = 0; i < 5; i++) {

        if (obj["ingredientLines"][i] != undefined) {
            ingrediants += obj["ingredientLines"][i] + "<br/>";
        }

    }

    return `
                <div class="col-md-4">
                    <div class="card mb-4 shadow-sm">
                        <img id="hell_llo" class="card-img-top" src="${obj["image"]}" />
                        <div class="card-body">
                            <h3>${obj["label"]}</h3>
                            <p class="card-text">${ingrediants}</p>
                            <div class="d-flex justify-content-between align-items-center">
                                <div class="btn-group">
                                    
                                            <form asp-action="FavouriteRecipe" method="post">
                                                <input type="hidden" id="label" name="label" value="${obj["label"]}" />
                                                <input type="hidden" id="image" name="image" value="${obj["image"]}" />
                                                <input type="hidden" id="ingrediants" name="ingrediants" value="${ingrediants}" />
                                                <input type="hidden" id="source" name="source" value="${obj["source"]}" />

                                                <button type="submit" class="btn btn-sm btn-outline-secondary">Favourite</button>
                                            </form>

                                </div>
                                <small class="text-muted">Provided By ${obj["source"]}</small>
                            </div>
                        </div>
                    </div>
                 </div>`;
}