document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('getOffersButton').onclick = streamResponse;
});

function streamResponse() {
    const offersContainer = document.getElementById('offersContainer');

    while (offersContainer.lastChild) {
        offersContainer.removeChild(offersContainer.lastChild);
    }

    const header = createHeader();
    const tbody = document.createElement("tbody");
    tbody.appendChild(header);

    const table = document.createElement("table");
    table.setAttribute("id", "offers");
    table.appendChild(tbody);

    offersContainer.appendChild(table);
    streamResponseTo(offersContainer, tbody)
}

/**
 * 
 * @param {HTMLElement} offersContainer
 * @param {HTMLTableSectionElement} tbody
 */
function streamResponseTo(offersContainer, tbody) {
    /** @type {HTMLInputElement} */
    // @ts-ignore
    const mixSuppliersElement = document.getElementById('mixSuppliers');
    /** @type {HTMLInputElement} */
    // @ts-ignore
    const maxResultsElement = document.getElementById('maxResults');
    /** @type {HTMLInputElement} */
    // @ts-ignore
    const errorChanceElement = document.getElementById('errorChance');

    const config = {
        'url': createUrl(mixSuppliersElement.checked, +maxResultsElement.value, +errorChanceElement.value),
        'method': 'GET',
        'cached': false
    };

    // @ts-ignore
    const oboeService = oboe(config);

    let offerId = 0;

    oboeService.node('!.*', function (response) {
        addResponse(response, tbody, offerId++, offersContainer);
    })
        .done(function () {
            const finishedContainer = document.createElement('p');
            finishedContainer.innerHTML = 'Finished...';
            offersContainer.appendChild(finishedContainer);
            finishedContainer.scrollIntoView();
        });
}

/**
 * @param {boolean} mixSuppliers
 * @param {number} maxResults
 * @param {number} errorChance
 * @returns {string}
 */
function createUrl(mixSuppliers, maxResults, errorChance) {
    let baseUrl = 'https://localhost:5270/hotels/offers-stream?';

    let hasParam = false;

    if (mixSuppliers) {
        baseUrl += 'mix-supplier-offers=' + mixSuppliers;
        hasParam = true;
    }

    if (maxResults) {
        if (hasParam) {
            baseUrl += '&'
        }

        baseUrl += 'max-results=' + maxResults;
        hasParam = true;
    }

    if (errorChance) {
        if (hasParam) {
            baseUrl += '&'
        }

        baseUrl += 'error-chance=' + errorChance;
        hasParam = true;
    }

    return baseUrl;
}

/**
 * 
 * @param {any} response
 * @param {HTMLTableSectionElement} tbody
 * @param {number} offerId
 * @param {HTMLElement} offersContainer
 */
function addResponse(response, tbody, offerId, offersContainer) {
    let cellData = document.createElement('td');
    cellData.innerHTML = `no. ${offerId + 1}`;

    const rowOffer = document.createElement('tr');
    rowOffer.id = offerId.toString();
    rowOffer.appendChild(cellData);

    if (response.value) {
        cellData = document.createElement('td');
        cellData.innerHTML += response.value.supplier;
        rowOffer.appendChild(cellData);

        cellData = document.createElement('td');
        cellData.innerHTML += response.value.id;
        rowOffer.appendChild(cellData);

        cellData = document.createElement('td');
        cellData.innerHTML += response.value.name;
        rowOffer.appendChild(cellData);
    }
    else if (response.error) {
        cellData = document.createElement('td');
        cellData.colSpan = 3;
        cellData.innerHTML += response.error.message;

        rowOffer.appendChild(cellData);
        rowOffer.setAttribute('has-error', 'true');
    }

    tbody.appendChild(rowOffer);
    rowOffer.scrollIntoView();
}

/**
 * @returns {HTMLTableRowElement}
 */
function createHeader() {
    const header = document.createElement('tr');

    let cellData = document.createElement('td');
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

    return header;
}