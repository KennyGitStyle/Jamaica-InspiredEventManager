import React, { Fragment, useContext, useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import NavBar from '../../features/nav/NavBar';
import EventActivityDashboard from '../../features/event-activities/dashboard/EventActivityDashboard';
import Homepage from '../../features/home/Homepage';
import { observer } from 'mobx-react-lite';
import { Route, withRouter, RouteComponentProps, Switch } from 'react-router-dom';
import EventActivityForm from '../../features/event-activities/event-form/EventActivityForm';
import EventActivityDetails from '../../features/event-activities/event-details/EventActivityDetails';
import NotFound from './NotFound';
import {ToastContainer} from 'react-toastify';
import LoginForm from '../../features/user/LoginForm';
import { RootStoreContext } from '../stores/rootStore';
import LoadingComponent from './LoadingComponent';
import ModalContainer from '../common/modals/ModalContainer';

interface AppProps {
  id: string
}

const App: React.FC<RouteComponentProps<AppProps>> = ({ location }) => {
  const rootStore = useContext(RootStoreContext)
  const {setAppLoaded, token, appLoaded} = rootStore.commonStore
  const {getUser} = rootStore.userStore

  useEffect(() => {
    token ? getUser().finally(() => setAppLoaded()) : setAppLoaded()
  },[token, getUser, setAppLoaded])

  if(!appLoaded){
    return <LoadingComponent content='Loading app...'/>
  }

  return (
    <Fragment>
      <ModalContainer />
      <ToastContainer position={'bottom-right'}/>
      <Route exact path='/' component={Homepage} />
      <Route path={'/(.+)'} render={() => (
        <Fragment>
          <NavBar />
          <Container style={{ marginTop: '5em' }}>
            <Switch>
              <Route exact path='/events' component={EventActivityDashboard} />
              <Route path='/events/:id' component={EventActivityDetails} />
              <Route key={location.key} path={['/createEvent', '/manage/:id']} component={EventActivityForm} />
              <Route path='/login' component={LoginForm} />
              <Route component={NotFound} />
            </Switch>
          </Container>
        </Fragment>
      )} />
    </Fragment>
  );
};

export default withRouter(observer(App));
