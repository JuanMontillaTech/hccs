function agregarEquipo() {
	var nombre = document.getElementById("nombre").value;
	var equipo = document.getElementById("equipo").value;
	var favorito = document.getElementById("favorito").checked;
	
	var tbody = document.getElementsByTagName("tbody")[0];
	var newRow = tbody.insertRow();
	var nombreCell = newRow.insertCell(0);
	var equipoCell = newRow.insertCell(1);
	var favoritoCell = newRow.insertCell(2);
	var accionesCell = newRow.insertCell(3);
	
	nombreCell.innerHTML = nombre;
	equipoCell.innerHTML = equipo;
	favoritoCell.innerHTML = favorito ? "Sí" : "No";
	accionesCell.innerHTML = "<button onclick=\"eliminarEquipo(this)\">Eliminar</button>";
}

function eliminarEquipo(row) {
	var index = row.parentNode.parentNode.rowIndex;
	document.getElementById("tabla").deleteRow(index);
}

function contarEquipos() {
	var aguilas = 0;
	var licey = 0;
	var escogido = 0;
	
	var tbody = document.getElementsByTagName("tbody")[0];
	var rows = tbody.getElementsByTagName("tr");
	for (var i = 0; i < rows.length; i++) {
		var equipoCell = rows[i].getElementsByTagName("td")[1];
		var equipo = equipoCell.innerHTML;
		
		switch (equipo) {
			case "Aguilas":
				aguilas++;
				break;
			case "Licey":
				licey++;
				break;
			case "Escogido":
				escogido++;
				break;
		}
	}
	
	return {
		"Aguilas": aguilas,
		"Licey": licey,
		"Escogido": escogido
	};
}