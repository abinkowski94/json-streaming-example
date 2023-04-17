document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('getOffersButton').onclick = streamResponse;
});

function streamResponse() {
    var mixSuppliers = document.getElementById('mixSuppliers').checked;
    var maxResults = document.getElementById('maxResults').value;
    
    const config = {
        'url': createUrl(mixSuppliers, maxResults),
        'method': 'GET',
        'cached': false
    };

    const oboeService = oboe(config);
    const offersContainer = document.getElementById('offersContainer');

    while (offersContainer.lastChild) {
        offersContainer.removeChild(offersContainer.lastChild);
    }

    var table = document.createElement("table");
    table.setAttribute("id", "offers");
    var tbody = document.createElement("tbody");
    offersContainer.appendChild(table);
    table.appendChild(tbody);

    let offerId = 0;

    createHeader(tbody);

    oboeService.node('!.*', function (response) {
        addResponse(response, tbody, offerId++);
    })
        .done(function () {
            const finishedContainer = document.createElement('p');
            finishedContainer.innerHTML = 'Finished...';
            offersContainer.appendChild(finishedContainer);
            finishedContainer.scrollIntoView();
        });
}

function createUrl(mixSuppliers, maxResults) {
    var baseUrl = 'http://localhost:5270/hotels/offers-stream?';
    var mixParameter = false;

    if (mixSuppliers) {
        baseUrl += 'mix-supplier-offers=' + mixSuppliers;
        mixParameter = true;
    }

    if (maxResults) {
        if (mixParameter) {
            baseUrl += '&'
        }
        baseUrl += 'max-results=' + maxResults;
    }

    return baseUrl;
}

function addResponse(response, tbody, offerId) {

    const rowOffer = document.createElement('tr');
    rowOffer.id = offerId;

    cellData = document.createElement('td');

    cellData.innerHTML = `no. ${rowOffer.id}`;
    rowOffer.appendChild(cellData);

    if (response.value) {
        cellData = document.createElement('td');
        cellData.innerHTML += `supplier: ${response.value.supplier}`;
        rowOffer.appendChild(cellData);
        cellData = document.createElement('td');
        cellData.innerHTML += response.value.id;
        rowOffer.appendChild(cellData);
        cellData = document.createElement('td');
        cellData.innerHTML += response.value.name;
        rowOffer.appendChild(cellData);
    }
    else if (response.error) {
        cellData = document.createElement('td')
        offersContainer.innerHTML += response.error.message;
        rowOffer.appendChild(cellData);
    }

    tbody.appendChild(rowOffer);
    offersContainer.scrollIntoView();
}

function createHeader(tbody) {
    const header = document.createElement('tr')
    var cellData = document.createElement('td');
    cellData.innerHTML += '<b>Number</b>';
    header.appendChild(cellData);
    cellData = document.createElement('td');
    cellData.innerHTML += '<b>Supplier</b>';
    header.appendChild(cellData);
    cellData = document.createElement('td');
    cellData.innerHTML += '<b>Identifier</b>';
    header.appendChild(cellData);
    cellData = document.createElement('td');
    cellData.innerHTML += '<b>Name</b>';
    header.appendChild(cellData);

    tbody.appendChild(header);
}