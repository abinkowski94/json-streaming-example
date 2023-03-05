document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('getOffersButton').onclick = streamResponse;
});

function streamResponse() {
    const config = {
        'url': 'http://localhost:5270/hotels/offers-stream?mix-supplier-offers=true',
        'method': 'GET',
        'cached': false
    };

    const oboeService = oboe(config);
    const offersContainer = document.getElementById('offersContainer');

    while (offersContainer.lastChild) {
        offersContainer.removeChild(offersContainer.lastChild);
    }

    let offerId = 1;

    oboeService.node('!.*', function (response) {
        addResponse(response, offersContainer, offerId);
    })
    .done(function () {
        const finishedContainer = document.createElement('p');
        finishedContainer.innerHTML = 'Finished...';
        offersContainer.appendChild(finishedContainer);
        finishedContainer.scrollIntoView();
    });
}

function addResponse(response, offersContainer, offerId) {
    const offerContainer = document.createElement('p');
    offerContainer.id = offerId;

    offerContainer.innerHTML = `no. ${offerContainer.id} | `;

    if (response.value) {
        offerContainer.innerHTML += `supplier: ${response.value.supplier} | `;
        offerContainer.innerHTML += response.value.id;
        offerContainer.innerHTML += ' - ';
        offerContainer.innerHTML += response.value.name;
    }
    else if (response.error) {
        offerContainer.innerHTML += response.error.message;
    }

    offersContainer.appendChild(offerContainer);
    offerContainer.scrollIntoView();
}