function GoBack() {
  window.history.back();
}

function OnSearchResultEndScroll(dotnetObject) {
  window.onscroll = function (ev) {
    if (
      window.innerHeight + Math.ceil(window.pageYOffset) >=
      document.body.offsetHeight
    ) {
      dotnetObject.invokeMethodAsync("SearchChefsOrDishesAndMenues");
    }
  };
}
