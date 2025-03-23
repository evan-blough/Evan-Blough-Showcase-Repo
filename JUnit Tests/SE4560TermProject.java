package org.apache.commons.collections4;

import static org.junit.jupiter.api.Assertions.assertThrowsExactly;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import org.apache.commons.collections4.list.FixedSizeList;
import org.apache.commons.collections4.list.GrowthList;
import org.apache.commons.collections4.list.SetUniqueList;
import org.apache.commons.collections4.map.SingletonMap;
import org.apache.commons.collections4.queue.CircularFifoQueue;
import org.apache.commons.collections4.set.ListOrderedSet;
import org.junit.Test;

public class SE4560TermProject {

    //#region Test Case #1: Boundaries of FIFO Queue Creation (Boundary Value)
    @Test
    public void circleFIFOCreate_IfSizeLessThanOrEqualTo0_ThrowIllegalArgumentException(){
        //All together
        assertThrowsExactly(IllegalArgumentException.class,
         () ->{CircularFifoQueue<Integer> q = new CircularFifoQueue<Integer>(0);});
    }

    @Test
    public void circleFIFOCreate_IfSizeIs1_CreateQueue(){
        CircularFifoQueue<Integer> q = new CircularFifoQueue<Integer>(1);

        // See if the queue was created
        assert(q.getClass().equals(CircularFifoQueue.class));
    }

    @Test
    public void circleFIFOCreate_IfNoSizeGiven_ThenSizeIs32(){
        CircularFifoQueue<Integer> q = new CircularFifoQueue<>();

        assert(q.maxSize() == 32);
    }

    @Test
    public void circleFIFOCreate_IfSizeIsGiven_ThenSizeEqualsParameter(){
        
        CircularFifoQueue<Integer> q = new CircularFifoQueue<>(999);

        assert(q.maxSize() == 999);
    }
    //#endregion

    //#region Test Case #2: FIFO Queue Circular Add (Boundary Value)
    @Test
    public void circleFIFOAdd_IfQueueIsNotFull_AddElementNormally(){
        // Arrange
        CircularFifoQueue<Integer> q = new CircularFifoQueue<Integer>(100);

        // Act
        for(Integer i = 0; i < 100; ++i) q.add(i);

        // Assert
        assert(q.peek() == 0);

        for (Integer i = 0; i < 99; ++i) q.remove();

        assert(q.peek() == 99);
    }
    @Test
    public void ifCircleFIFOIsFull_AndNewElementAdded_ThenRemoveYoungest(){
        // Arrange
        CircularFifoQueue<Integer> q = new CircularFifoQueue<Integer>(5);

        // Act
        //Fill up queue
        for (Integer i = 0; i < 5; i++) q.add(i);
        q.add(6);

        // Assert
        assert(q.isAtFullCapacity() && q.contains(6) && !q.contains(5));
    }
    //#endregion

    //#region Test Case #3: FIFO Queue Fullness (Boundary Value)
    @Test
    public void circleFIFOFullness_HasOpenSpots_ThenQueueIsNotFull(){
        // Arrange
        CircularFifoQueue<Integer> q = new CircularFifoQueue<Integer>(5);

        // Act
        for (Integer i = 0; i < 4; i++) q.add(i);

        assert(!q.isAtFullCapacity() && !q.isEmpty());
    }

    @Test
    public void circleFIFOFullness_HasNoOpenSpots_ThenQueueIsFull(){
        // Arrange
        CircularFifoQueue<Integer> q = new CircularFifoQueue<Integer>(5);

        // Act
        for (Integer i = 0; i < 5; i++) q.add(i);

        // Assert
        assert(q.isAtFullCapacity() && !q.isEmpty());
    }

    @Test
    public void circleFIFOFullness_AllElementsAreRemoved_ThenQueueIsCalledEmpty(){
        // Arrange
        CircularFifoQueue<Integer> q = new CircularFifoQueue<Integer>(5);

        // Act
        for (Integer i = 0; i < 5; i++) q.add(i);
        assert(q.isAtFullCapacity());

        for (Integer i = 0; i < 5; i++) q.remove();

        // Assert
        assert(!q.isAtFullCapacity() && q.isEmpty());
    }
    //#endregion

    //#region Test Case #4: Boundaries of Growth List Creation (Boundary Value)
    @Test
    public void growthListCreate_IfCreationSizeIs0_ThenSuccessfullyCreate(){
        // Arrange
        GrowthList<Integer> l = new GrowthList<Integer>(0);
        
        // Assert
        assert(l.size() == 0 && l.getClass().equals(GrowthList.class));
    }

    @Test 
    public void growthListCreate_IfCreationSizeIsNegative_ThenThrowError(){
        // Arrange
        assertThrowsExactly(IllegalArgumentException.class,
         () -> {GrowthList<Integer> l = new GrowthList<Integer>(-1);});
    }

    @Test
    public void growthListCreate_IfCreationSizeIsNotGiven_CreateList(){
        // Arrange
        GrowthList<Integer> l = new GrowthList<Integer>();
        
        // Assert
        assert(l.size() == 0 && l.getClass().equals(GrowthList.class));
    }

    @Test
    public void growthListCreate_IfCapacityIsGiven_(){
        // Arrange
        GrowthList<Integer> l = new GrowthList<Integer>(50);
        
        // Assert
        assert(l.size() == 0 && l.getClass().equals(GrowthList.class));
    }
    //#endregion

    //#region Test Case #5: Function add(element, value) Paths For Growth List (Path Testing)
    @Test
    public void growthListAdd_IfSizeIsSameAsIndexOfNewElement_ThenListGrowsToAccommodate(){
        // Arrange
        GrowthList<Integer> l = new GrowthList<Integer>(0);

        // Act
        assert(l.size() == 0);
        l.add(0, 0);

        // Assert
        assert(l.get(0) == 0 && l.size() == 1);
    }

    @Test
    public void growthListAdd_IfElementToAddIsOccupied_PushFirstElement(){
        
        // Arrange
        GrowthList<Integer> l = new GrowthList<Integer>(1);

        // Act
        l.add(0, 1);
        assert(l.get(0) == 1);
        l.add(0, 99);

        // Assert
        assert(l.get(0) == 99 && l.get(1) == 1);
    }

    @Test
    public void growthListAdd_IfElementToAddIndexIsOutOfRange_GrowList(){
        
        // Arrange
        GrowthList<Integer> l = new GrowthList<Integer>(1);

        // Act
        l.add(999, 99);

        // Assert
        assert(l.get(999) == 99);
    }

    @Test
    public void growthListAdd_IfElementIsInNormalSize_AddElement(){
        
        // Arrange
        GrowthList<Integer> l = new GrowthList<Integer>(999);

        // Act
        l.add(500, 99);

        // Assert
        assert(l.get(500) == 99);

    }
    //#endregion

    //#region Test Case #6: SetUniqueList Add Testing (Partitioning)
    @Test
    public void setUniqueListAdd_IfDoesNotExistInList_ThenAddElement(){
        // Arrange
        SetUniqueList<Integer> sul = SetUniqueList.setUniqueList(
            new ArrayList<Integer>(){{add(1);add(2);add(3); add(4); add(5);}});

        // Act
        sul.add(6);
        // Assert
        assert(sul.contains(6) && sul.size() == 6);
    }

    @Test
    public void setUniqueListAdd_IfExistsInList_ThenThrowException(){
        // Arrange
        SetUniqueList<Integer> sul = SetUniqueList.setUniqueList(
            new ArrayList<Integer>(){{add(1);add(2);add(3); add(4); add(5);}});
        ArrayList<Integer> compare = new ArrayList<Integer>();

        // Act
        compare.addAll(sul);
        sul.add(5);

        // Assert
        assert(sul.equals(compare));
    }

    @Test
    public void setUniqueListAddAll_IfAllCollectionDoesNotExistInList_AddAll(){
        // Arrange
        SetUniqueList<Integer> sul = SetUniqueList.setUniqueList(
            new ArrayList<Integer>(){{add(1);add(2);add(3); add(4); add(5);}});
        ArrayList<Integer> toAdd = new ArrayList<Integer>(){{add(100); add(99); add (98);}};

        // Act
        sul.addAll(toAdd);

        // Assert
        assert(sul.containsAll(toAdd));
    }

    @Test 
    public void setUniqueListAddAll_IfPartOfCollectionExistsInList_ThenIgnoreAlreadyAdded(){
        // Arrange
        SetUniqueList<Integer> sul = SetUniqueList.setUniqueList(
            new ArrayList<Integer>(){{add(1);add(2);add(3); add(4); add(5);}});
        ArrayList<Integer> toAdd = new ArrayList<Integer>(){{add(100); add(4); add (98);}};

        // Act
        sul.addAll(toAdd);

        // Assert
        assert(sul.containsAll(toAdd) && sul.size() == 7);
    }

    @Test
    public void setUniqueListAddAll_IfCollectionExistsInList_ThenIgnoreAll(){
        // Arrange
        SetUniqueList<Integer> sul = SetUniqueList.setUniqueList(
        new ArrayList<Integer>(){{add(1);add(2);add(3); add(4); add(5);}});
        ArrayList<Integer> toAdd = new ArrayList<Integer>(){{add(5); add(4); add (1);}};
        
        // Act
        sul.addAll(toAdd);
        
        // Assert
        assert(sul.containsAll(toAdd) && sul.size() == 5);
    }
    //#endregion

    //#region Test Case #7: ListOrderedSet Creation (Path Testing)
    @Test
    public void listOrderedSetCreation_IfSetAndListAreEmpty_ThenCreateListOrderedSet(){
        // Arrange
        List<Integer> l = new ArrayList<Integer>();
        Set<Integer> s = new HashSet<Integer>();

        // Act
        ListOrderedSet<Integer> los = ListOrderedSet.listOrderedSet(s, l);

        // Assert
        assert(los.isEmpty() && los.getClass().equals(ListOrderedSet.class));
    }

    @Test
    public void listOrderedSetCreation_IfSetIsNull_ThenThrowException(){
        // Arrange
        List<Integer> l = new ArrayList<Integer>();
        Set<Integer> s = null;

        // Assert
        assertThrowsExactly(NullPointerException.class, 
        () -> {ListOrderedSet<Integer> los = ListOrderedSet.listOrderedSet(s, l);});
    }

    @Test
    public void listOrderedSetCreation_IfListIsNull_ThenThrowException(){
        // Arrange
        List<Integer> l = null;
        Set<Integer> s = new HashSet<Integer>();

        // Assert
        assertThrowsExactly(NullPointerException.class, 
        () -> {ListOrderedSet<Integer> los = ListOrderedSet.listOrderedSet(s, l);});
    }

    @Test
    public void listOrderedSetCreation_IfSetHasContents_ThenThrowException(){
        // Arrange
        List<Integer> l = new ArrayList<Integer>();
        Set<Integer> s = new HashSet<Integer>(){{add(99);}};

        // Assert
        assertThrowsExactly(IllegalArgumentException.class, 
        () -> {ListOrderedSet<Integer> los = ListOrderedSet.listOrderedSet(s, l);});
    }

    @Test
    public void listOrderedSetCreation_IfListHasContents_ThenThrowException(){
        // Arrange
        List<Integer> l = new ArrayList<Integer>(){{add(99);}};
        Set<Integer> s = new HashSet<Integer>();

        // Assert
        assertThrowsExactly(IllegalArgumentException.class, 
        () -> {ListOrderedSet<Integer> los = ListOrderedSet.listOrderedSet(s, l);});
    }
    //#endregion

    //#region Test Case #8: SingletonMap equivalence testing (Path Testing)
    @Test
    public void singletonMapEqual_IfSingletonAndMapBothSize1WithSameKeyValuePair_ThenTrue(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>("key", 1);
        HashMap<String, Integer> map = new HashMap<String, Integer>();
        // Act
        map.put("key", 1);

        // Assert
        assert(singleton.equals(map));
    }
    
    @Test
    public void singletonMapEqual_IfMapIsGreaterThanSize1_ThenFalse(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>("key", 1);
        HashMap<String, Integer> map = new HashMap<String, Integer>();
        // Act
        map.put("key", 1);
        map.put(null, null);

        // Assert
        assert(!singleton.equals(map));
    }

    @Test
    public void singletonMapEqual_IfSingletonIsComparedToSelf_ThenTrue(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>("key", 1);

        // Assert
        assert(singleton.equals(singleton));
    }
    
    @Test
    public void singletonMapEqual_IfBothSingletonButKeyIsDifferent_ThenFalse(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>("key", 1);
        SingletonMap<String, Integer> map = new SingletonMap<String, Integer>("key ", 1);

        // Assert
        assert(!singleton.equals(map));
    }
    
    @Test
    public void singletonMapEqual_IfBothSingletonButValueIsDifferent_ThenFalse(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>("key", 1);
        SingletonMap<String, Integer> map = new SingletonMap<String, Integer>("key", 11);

        // Assert
        assert(!singleton.equals(map));
    }

    @Test
    public void singletonMapEqual_IfObjectToCompareIsNotMap_ThenFalse(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>();
        ArrayList<String> obj = new ArrayList<String>();
        // Act
        obj.add(null);

        // Assert
        assert(!singleton.equals(obj));
    }
    //#endregion

    //#region Test Case #9: SingletonMap Creation and Manipulation (Partition?)
    @Test
    public void singletonMapCreation_IfNoParametersArePassed_ThenKeyAndValueAreNull(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>();

        // Assert
        assert(singleton.getKey() == null && singleton.getValue() == null);
    }

    @Test
    public void singletonMapCreation_IfKeyIsGivenAndThenValueChanged_Valid(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>("key", null);

        // Act
        singleton.put("key", 1);

        // Assert
        assert(singleton.get("key") == 1);
    }

    @Test
    public void singletonMapCreation_IfNewKeyValuePassed_ThrowException(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>();

        // Assert
        assertThrowsExactly(IllegalArgumentException.class, () -> {singleton.put("key", 1);});
    }

    @Test
    public void singletonMapCreation_IfNewKeyPassed_ThrowException(){
        // Arrange
        SingletonMap<String, Integer> singleton = new SingletonMap<String, Integer>("key", 1);

        // Assert
        assertThrowsExactly(IllegalArgumentException.class, () -> {singleton.put("new key", null);});
    }

    //#endregion

    //#region Test Case #10: Fixed Size List Edge Tests (Boundary?)
    @Test
    public void fixedSizeList_IfTryToAddElementWhenFull_ThrowException(){
        // Arrange
        FixedSizeList<Integer> l = FixedSizeList.fixedSizeList(
            new ArrayList<Integer>(){{add(1); add(2); add(3);}});
        // Assert
        assert(l.isFull());
        assertThrowsExactly(UnsupportedOperationException.class, () -> {l.add(4);});

    }

    @Test
    public void fixedSizeList_IfTryToRemoveAnElement_ThrowException(){
        // Arrange
        FixedSizeList<Integer> l = FixedSizeList.fixedSizeList(
            new ArrayList<Integer>(){{add(1); add(2); add(3);}});
        // Assert
        assertThrowsExactly(UnsupportedOperationException.class, () -> {l.remove(1);});
    }

    @Test
    public void fixedSizeList_IfTryToChangeAnElement_ElementIsChanged(){
        // Arrange
        FixedSizeList<Integer> l = FixedSizeList.fixedSizeList(
            new ArrayList<Integer>(){{add(1); add(2); add(3);}});
        // Act
        l.set(0, 9);
        // Assert
        assert(l.contains(9) && !l.contains(1));
    }
    //#endregion
}