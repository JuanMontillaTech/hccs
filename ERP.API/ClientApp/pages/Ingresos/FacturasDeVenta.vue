<template>
  <FuntionalList
    Title="Facturas"
    TransactionsType="1"
    Form="factura"
    IsClient="true"
    DateLabel="Fecha de Factura"
    :Path="$route.path"
     DocumentTypeId="2"
  ></FuntionalList>
</template>
