function toggleDateRangeFields() {
    var isDateRangeChecked = document.querySelector('input[name="IsDateRange"]').checked;
    document.getElementById("dateRangeFields").style.display = isDateRangeChecked ? "block" : "none";
}

function addDateRange() {
    var container = document.getElementById("dateRangesContainer");
    var index = container.children.length;

    var rangeDiv = document.createElement("div");
    rangeDiv.className = "date-range";
    rangeDiv.innerHTML = `
        <div>
            <label>Дата начала диапазона</label>
            <input type="datetime-local" name="DateRanges[${index}].StartDate" />
        </div>
        <div>
            <label>Дата конца диапазона</label>
            <input type="datetime-local" name="DateRanges[${index}].EndDate" />
        </div>
        <button type="button" onclick="this.parentNode.remove()">Удалить</button>
        <hr />
    `;

    container.appendChild(rangeDiv);
}

document.addEventListener("DOMContentLoaded", function () {
    toggleDateRangeFields();

    if (document.querySelector('input[name="IsDateRange"]').checked) {
        addDateRange();
    }

    document.querySelector('input[name="IsDateRange"]')
        .addEventListener("change", toggleDateRangeFields);
});