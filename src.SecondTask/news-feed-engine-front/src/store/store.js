import { applyMiddleware } from "redux";
import { configureStore as createStore } from "@reduxjs/toolkit";
import thunk from "redux-thunk";
import rootReducer from "./reducers/rootReducer";

const configureStore = () => {
	const configurated = createStore(
		{ reducer: rootReducer },
		applyMiddleware(thunk)
	);

	return configurated;
};

export default configureStore;
