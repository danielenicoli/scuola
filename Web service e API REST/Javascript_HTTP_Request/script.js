document.addEventListener('DOMContentLoaded', () => {
    const fetchButton = document.getElementById('fetchButton');
    const barcodeInput = document.getElementById('barcode');
    const responseContainer = document.getElementById('responseContainer');
    const imageContainer = document.getElementById('imageContainer');

    fetchButton.addEventListener('click', () => {
        const barcode = barcodeInput.value;
        fetch('https://world.openfoodfacts.net/api/v2/product/' + barcode + '/?fields=product_name,brands,quantity,image_url,ingredients')
            .then(response => {
                if (response.status == 200) {
                    return response.json();
                }
            })
            .then(data => {
                console.log(data);
                responseContainer.innerHTML = data.product.product_name + " (marca " + data.product.brands + ", peso " + data.product.quantity + ")";
                imageContainer.src = data.product.image_url;
            })
            .catch(error => {
                console.error('Errore: ', error);
                responseContainer.innerHTML = 'Si è verificato un errore';
            });
    });
});
