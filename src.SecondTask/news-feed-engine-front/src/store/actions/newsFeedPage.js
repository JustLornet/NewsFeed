import * as api from "../api";

export const SET_REDUX_POSTS = "SET_REDUX_POSTS";
const setReduxPosts = posts => {};

export const REQUEST_DATA = "REQUEST_DATA";
export const requestData = () => {
	return { type: REQUEST_DATA };
};

export const fetchPostsFromBack = () => {
	return dispatch => {
		dispatch(requestData());
		api.get(`/NewsFeed/GetPosts`)
			.then(response => {
				if (response.status != 200) {
					console.log("Error", response);
					return [];
				}

				return response.data;
			})
			.then(json => dispatch(setReduxPosts(json)));
	};
};
