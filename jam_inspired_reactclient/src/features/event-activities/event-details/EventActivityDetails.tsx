import React, { useContext, useEffect } from 'react';
import { Grid } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import EventActivityDetailedHeader from './EventActivityDetailedHeader';
import EventActivityDetailedInfo from './EventActivityDetailedInfo';
import EventActivityDetailedChat from './EventActivityDetailedChat';
import EventActivityDetailedSidebar from './EventActivityDetailedSidebar';
import { RootStoreContext } from '../../../app/stores/rootStore';

interface DetailsParams {
  id: string
}

const EventActivityDetails: React.FC<RouteComponentProps<DetailsParams>> = ({ match, history }) => {
  const rootStore = useContext(RootStoreContext);
  const { activity, loadEventActivity, loadingInitial } = rootStore.eventActivityStore

  useEffect(() => {
    loadEventActivity(match.params.id);
  }, [loadEventActivity, match.params.id, history])

  if (loadingInitial) return <LoadingComponent content='loading event...' />

  if(!activity) return <h2>Event Not Found!</h2>

  return (
    <Grid>
      <Grid.Column width={10}>
        <EventActivityDetailedHeader event={activity} />
        <EventActivityDetailedInfo activity={activity} />
        <EventActivityDetailedChat />
      </Grid.Column>
      <Grid.Column width={6}>
        <EventActivityDetailedSidebar />
      </Grid.Column>
    </Grid>
  );
};

export default observer(EventActivityDetails);
