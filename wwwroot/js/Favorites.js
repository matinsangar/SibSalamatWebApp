function addToFavorites(productName) {
    $.ajax({
        type: "POST",
        url: "/Account/AddToFavorites",
        data: {productName: productName},
        success: function (result) {
            if (result.success) {
                alert("Product added to favorites!");
            } else {
                alert("Failed to add product to favorites.");
            }
        },
        error: function () {
            alert("An error occurred while adding the product to favorites.");
        }
    });
}
