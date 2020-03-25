import React, {useContext, useEffect} from 'react';
import { Grid } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import EventActivityList from './EventActivityList';
import { observer } from 'mobx-react-lite';
import { RootStoreContext } from '../../../app/stores/rootStore';

const EventActivityDashboard: React.FC = () => {
  const rootStore =  useContext(RootStoreContext)
  const {loadEventActivities, loadingInitial} = rootStore.eventActivityStore

  useEffect(() => {
    loadEventActivities()
  }, [loadEventActivities]);

  if(loadingInitial) return <LoadingComponent content='loading event activities...'/>
  return (
    <Grid>
      <Grid.Column width={10}>
        <EventActivityList />
      </Grid.Column>
      <Grid.Column width={6}>
        <h2>Event filters</h2>
      </Grid.Column>
    </Grid>
  );
};

export default observer(EventActivityDashboard);
