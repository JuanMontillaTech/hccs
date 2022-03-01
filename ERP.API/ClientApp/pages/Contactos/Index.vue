<template>
    <div class="container">
 
<datatable
	title="Basic table"
	:columns="tableColumns1"
	:rows="tableRows1"
/>
						
<vue-materialize-datatsable
	title="Basic table"
	:columns="tableColumns1"
	:rows="tableRows1"
/>
    </div>
</template>

<script>
 
 export default {
   
data() {
	return {
		tableColumns1: [
			{
				label: "Character name",
				field: "charName",
				numeric: false,
				html: false
			},
			{
				label: "First appearance",
				field: "firstAppearance",
				numeric: false,
				html: false
			},
			{
				label: "Created by",
				field: "createdBy",
				numeric: false,
				html: false
			},
			{
				label: "Voiced by",
				field: "voicedBy",
				numeric: false,
				html: false
			}
		],
		tableRows1: [
			{
				charName: "Abu",
				firstAppearance: "Alladin (1992)",
				createdBy: "Joe Grant",
				voicedBy: "Frank Welker"
			},
			{
				charName: "Magic Carpet",
				firstAppearance: "Alladin (1992)",
				createdBy: "Randy Cartwright",
				voicedBy: "N/A"
			},
			{
				charName: "The Sultan",
				firstAppearance: "Alladin (1992)",
				createdBy: "Navid Negahban",
				voicedBy: "Douglas Seale"
			}
		],
	}
}
						
}
</script>

<style>

</style>