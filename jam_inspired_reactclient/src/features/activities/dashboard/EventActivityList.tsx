import React, { useContext, Fragment } from 'react';
import { observer } from 'mobx-react-lite';
import { Item, Label } from 'semantic-ui-react';
import EventActivityListItem from './EventActivityListItem';
import {  RootStoreContext } from '../../../app/stores/rootStore';
import {format} from 'date-fns';


const EventActivityList: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { eventActivitiesByDate } = rootStore.eventActivityStore;
  return (
    <Fragment>
      {eventActivitiesByDate.map(([group, events]) => (
        <Fragment key={group}>
          <Label size='large' color='green'>
          {format(Date.parse(group), 'eeee do MMMM')}
          </Label>
          <Item.Group divided>
            {events.map(event => (
              <EventActivityListItem key={event.id} event={event} />
            ))}
          </Item.Group>
        </Fragment>
      ))}
    </Fragment>
  );
};

export default observer(EventActivityList);
