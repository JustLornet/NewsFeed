import { connect } from "react-redux"
import { fetchPostsFromBack } from '../store/actions/newsFeedPage'
import { useEffect } from "react"

const NewsFeedPage = ({ posts, fetchPosts = (f) => f }) => {
    useEffect(() => {
        if(posts == null) {
            fetchPosts(1)
        }
    }, [])

    return (
        <div>

        </div>
    )
}

const mapStateToProps = (state) => {
    return {
        posts: state?.posts
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchPosts: (usersIds, postsAmount) => dispatch(fetchPostsFromBack(usersIds, postsAmount))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NewsFeedPage)